using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;

namespace SnackBarService.ImplementationsList
{
    public class MainServiceList : InterfaceMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<OrderViewModel> getList()
        {
            List<OrderViewModel> result = new List<OrderViewModel>();
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                string clientFullName = string.Empty;
                for (int j = 0; j < source.Clients.Count; ++j)
                {
                    if (source.Clients[j].ID == source.Orders[i].ClientID)
                    {
                        clientFullName = source.Clients[j].ClientFullName;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Products.Count; ++j)
                {
                    if (source.Products[j].ID == source.Orders[i].ProductID)
                    {
                        productName = source.Products[j].ProductName;
                        break;
                    }
                }
                string executorFullName = string.Empty;
                if (source.Orders[i].ExecutorID.HasValue)
                {
                    for (int j = 0; j < source.Executors.Count; ++j)
                    {
                        if (source.Executors[j].ID == source.Orders[i].ExecutorID.Value)
                        {
                            executorFullName = source.Executors[j].ExecutorFullName;
                            break;
                        }
                    }
                }
                result.Add(new OrderViewModel
                {
                    ID = source.Orders[i].ID,
                    ClientID = source.Orders[i].ClientID,
                    ClientFullName = clientFullName,
                    ProductID = source.Orders[i].ProductID,
                    ProductName = productName,
                    ExecutorID = source.Orders[i].ExecutorID,
                    ExecutorName = executorFullName,
                    Count = source.Orders[i].Count,
                    Summa = source.Orders[i].Summa,
                    DateOfCreate = source.Orders[i].DateOfCreate.ToLongDateString(),
                    DateOfImplement = source.Orders[i].DateOfImplement?.ToLongDateString(),
                    Status = source.Orders[i].Status.ToString()
                });
            }
            return result;
        }

        public void createOrder(BoundOrderModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Orders[i].ID > maxID)
                    maxID = source.Clients[i].ID;
            }
            source.Orders.Add(new Order
            {
                ID = maxID + 1,
                ClientID = model.ClientID,
                ProductID = model.ProductID,
                DateOfCreate = DateTime.Now,
                Count = model.Count,
                Summa = model.Summa,
                Status = StatusOfOrder.Принятый
            });
        }

        public void takeOrderInWork(BoundOrderModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Orders[i].ID == model.ID)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ProductID == source.Orders[index].ProductID)
                {
                    int countOnStocks = 0;
                    for (int j = 0; j < source.ReserveElements.Count; ++j)
                    {
                        if (source.ReserveElements[j].ElementID == source.ProductElements[i].ElementID)
                            countOnStocks += source.ReserveElements[j].Count;
                    }
                    if (countOnStocks < source.ProductElements[i].Count * source.Orders[index].Count)
                    {
                        for (int j = 0; j < source.Elements.Count; ++j)
                        {
                            if (source.Elements[j].ID == source.ProductElements[i].ElementID)
                                throw new Exception("Не достаточно компонента " + source.Elements[j].ElementName +
                                    " требуется " + source.ProductElements[i].Count + ", в наличии " + countOnStocks);
                        }
                    }
                }
            }
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ProductID == source.Orders[index].ProductID)
                {
                    int countOnStocks = source.ProductElements[i].Count * source.Orders[index].Count;
                    for (int j = 0; j < source.ReserveElements.Count; ++j)
                    {
                        if (source.ReserveElements[j].ElementID == source.ProductElements[i].ElementID)
                        {
                            if (source.ReserveElements[j].Count >= countOnStocks)
                            {
                                source.ReserveElements[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.ReserveElements[j].Count;
                                source.ReserveElements[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Orders[index].ExecutorID = model.ExecutorID;
            source.Orders[index].DateOfImplement = DateTime.Now;
            source.Orders[index].Status = StatusOfOrder.Выполняемый;
        }

        public void finishOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Clients[i].ID == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Orders[index].Status = StatusOfOrder.Готов;
        }

        public void payOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Orders.Count; ++i)
            {
                if (source.Clients[i].ID == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Orders[index].Status = StatusOfOrder.Оплаченный;
        }

        public void putComponentOnReserve(BoundResComponentModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.ReserveElements.Count; ++i)
            {
                if (source.ReserveElements[i].ReserveID == model.ReserveID &&
                    source.ReserveElements[i].ElementID == model.ElementID)
                {
                    source.ReserveElements[i].Count += model.Count;
                    return;
                }
                if (source.ReserveElements[i].ID > maxID)
                    maxID = source.ReserveElements[i].ID;
            }
            source.ReserveElements.Add(new ReserveElement
            {
                ID = ++maxID,
                ReserveID = model.ReserveID,
                ElementID = model.ElementID,
                Count = model.Count
            });
        }
    }
}
