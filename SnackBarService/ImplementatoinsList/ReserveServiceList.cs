using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;

namespace SnackBarService.ImplementationsList
{
    public class ReserveServiceList : InterfaceReserveService
    {
        private DataListSingleton source;

        public ReserveServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelReserveView> getList()
        {
            List<ModelReserveView> result = new List<ModelReserveView>();
            for (int i = 0; i < source.Reserves.Count; ++i)
            {
                List<ModelReserveElementView> StockComponents = new List<ModelReserveElementView>();
                for (int j = 0; j < source.ReserveElements.Count; ++j)
                {
                    if (source.ReserveElements[j].ReserveID == source.Reserves[i].ID)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.ProductElements[j].ElementID == source.Elements[k].ID)
                            {
                                componentName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        StockComponents.Add(new ModelReserveElementView
                        {
                            ID = source.ReserveElements[j].ID,
                            ReserveID = source.ReserveElements[j].ReserveID,
                            ElementID = source.ReserveElements[j].ElementID,
                            ElementName = componentName,
                            Count = source.ReserveElements[j].Count
                        });
                    }
                }
                result.Add(new ModelReserveView
                {
                    ID = source.Reserves[i].ID,
                    ReserveName = source.Reserves[i].ReserveName,
                    ReserveElements = StockComponents
                });
            }
            return result;
        }

        public ModelReserveView getElement(int id)
        {
            for (int i = 0; i < source.Reserves.Count; ++i)
            {
                List<ModelReserveElementView> StockComponents = new List<ModelReserveElementView>();
                for (int j = 0; j < source.ReserveElements.Count; ++j)
                {
                    if (source.ReserveElements[j].ReserveID == source.Reserves[i].ID)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.ProductElements[j].ElementID == source.Elements[k].ID)
                            {
                                componentName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        StockComponents.Add(new ModelReserveElementView
                        {
                            ID = source.ReserveElements[j].ID,
                            ReserveID = source.ReserveElements[j].ReserveID,
                            ElementID = source.ReserveElements[j].ElementID,
                            ElementName = componentName,
                            Count = source.ReserveElements[j].Count
                        });
                    }
                }
                if (source.Reserves[i].ID == id)
                {
                    return new ModelReserveView
                    {
                        ID = source.Reserves[i].ID,
                        ReserveName = source.Reserves[i].ReserveName,
                        ReserveElements = StockComponents
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundReserveModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Reserves.Count; ++i)
            {
                if (source.Reserves[i].ID > maxID)
                    maxID = source.Reserves[i].ID;
                if (source.Reserves[i].ReserveName == model.ReserveName)
                    throw new Exception("Уже есть склад с таким названием");
            }
            source.Reserves.Add(new Reserve
            {
                ID = maxID + 1,
                ReserveName = model.ReserveName
            });
        }

        public void updateElement(BoundReserveModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Reserves.Count; ++i)
            {
                if (source.Reserves[i].ID == model.ID)
                    index = i;
                if (source.Reserves[i].ReserveName == model.ReserveName && source.Reserves[i].ID != model.ID)
                    throw new Exception("Уже есть склад с таким названием");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Reserves[index].ReserveName = model.ReserveName;
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.ReserveElements.Count; ++i)
            {
                if (source.ReserveElements[i].ReserveID == id)
                    source.ReserveElements.RemoveAt(i--);
            }
            for (int i = 0; i < source.Reserves.Count; ++i)
            {
                if (source.Reserves[i].ID == id)
                {
                    source.Reserves.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
