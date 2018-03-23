using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class MainServiceList : InterfaceMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelBookingView> getList()
        {
            List<ModelBookingView> result = source.Bookings
                .Select(rec => new ModelBookingView
                {
                    ID = rec.ID,
                    CustomerID = rec.CustomerID,
                    OutputID = rec.OutputID,
                    ExecutorID = rec.ExecutorID,
                    DateOfCreate = rec.DateOfCreate.ToLongDateString(),
                    DateOfImplement = rec.DateOfImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Summa = rec.Summa,
                    CustomerFullName = source.Customers
                                    .FirstOrDefault(recC => recC.ID == rec.CustomerID)?.CustomerFullName,
                    OutputName = source.Outputs
                                    .FirstOrDefault(recP => recP.ID == rec.OutputID)?.OutputName,
                    ExecutorName = source.Executors
                                    .FirstOrDefault(recI => recI.ID == rec.ExecutorID)?.ExecutorFullName
                })
                .ToList();
            return result;
        }

        public void createOrder(BoundBookingModel model)
        {
            int maxId = source.Bookings.Count > 0 ? source.Bookings.Max(rec => rec.ID) : 0;
            source.Bookings.Add(new Booking
            {
                ID = maxId + 1,
                CustomerID = model.CustomerID,
                OutputID = model.OutputID,
                DateOfCreate = DateTime.Now,
                Count = model.Count,
                Summa = model.Summa,
                Status = StatusOfBooking.Принятый
            });
        }

        public void takeOrderInWork(BoundBookingModel model)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            var productComponents = source.OutputElements.Where(rec => rec.OutputID == element.OutputID);
            foreach (var productComponent in productComponents)
            {
                int countOnStocks = source.ReserveElements
                                            .Where(rec => rec.ElementID == productComponent.ElementID)
                                            .Sum(rec => rec.Count);
                if (countOnStocks < productComponent.Count * element.Count)
                {
                    var componentName = source.Elements
                                    .FirstOrDefault(rec => rec.ID == productComponent.ElementID);
                    throw new Exception("Не достаточно компонента " + componentName?.ElementName +
                        " требуется " + productComponent.Count + ", в наличии " + countOnStocks);
                }
            }
            foreach (var productComponent in productComponents)
            {
                int countOnStocks = productComponent.Count * element.Count;
                var stockComponents = source.ReserveElements
                                            .Where(rec => rec.ElementID == productComponent.ElementID);
                foreach (var stockComponent in stockComponents)
                {
                    if (stockComponent.Count >= countOnStocks)
                    {
                        stockComponent.Count -= countOnStocks;
                        break;
                    }
                    else
                    {
                        countOnStocks -= stockComponent.Count;
                        stockComponent.Count = 0;
                    }
                }
            }
            element.ExecutorID = model.ExecutorID;
            element.DateOfImplement = DateTime.Now;
            element.Status = StatusOfBooking.Выполняемый;
        }

        public void finishOrder(int id)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.ID == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = StatusOfBooking.Готов;
        }

        public void payOrder(int id)
        {
            Booking element = source.Bookings.FirstOrDefault(rec => rec.ID == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = StatusOfBooking.Оплаченный;
        }

        public void putComponentOnReserve(BoundResElementModel model)
        {
            ReserveElement element = source.ReserveElements
                                                .FirstOrDefault(rec => rec.ReserveID == model.ReserveID &&
                                                                    rec.ElementID == model.ElementID);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.ReserveElements.Count > 0 ? source.ReserveElements.Max(rec => rec.ID) : 0;
                source.ReserveElements.Add(new ReserveElement
                {
                    ID = ++maxId,
                    ReserveID = model.ElementID,
                    ElementID = model.ElementID,
                    Count = model.Count
                });
            }
        }
    }
}
