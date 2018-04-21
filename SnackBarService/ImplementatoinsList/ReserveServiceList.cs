using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<ModelReserveView> result = source.Reserves
                .Select(rec => new ModelReserveView
                {
                    ID = rec.ID,
                    ReserveName = rec.ReserveName,
                    ReserveElements = source.ReserveElements
                            .Where(recPC => recPC.ReserveID == rec.ID)
                            .Select(recPC => new ModelReserveElementView
                            {
                                ID = recPC.ID,
                                ReserveID = recPC.ReserveID,
                                ElementID = recPC.ElementID,
                                ElementName = source.Elements
                                    .FirstOrDefault(recC => recC.ID == recPC.ElementID)?.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ModelReserveView getElement(int id)
        {
            Reserve element = source.Reserves.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelReserveView
                {
                    ID = element.ID,
                    ReserveName = element.ReserveName,
                    ReserveElements = source.ReserveElements
                            .Where(recPC => recPC.ReserveID == element.ID)
                            .Select(recPC => new ModelReserveElementView
                            {
                                ID = recPC.ID,
                                ReserveID = recPC.ReserveID,
                                ElementID = recPC.ElementID,
                                ElementName = source.Elements
                                    .FirstOrDefault(recC => recC.ID == recPC.ElementID)?.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundReserveModel model)
        {
            Reserve element = source.Reserves.FirstOrDefault(rec => rec.ReserveName == model.ReserveName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Reserves.Count > 0 ? source.Reserves.Max(rec => rec.ID) : 0;
            source.Reserves.Add(new Reserve
            {
                ID = maxId + 1,
                ReserveName = model.ReserveName
            });
        }

        public void updateElement(BoundReserveModel model)
        {
            Reserve element = source.Reserves.FirstOrDefault(rec =>
                                        rec.ReserveName == model.ReserveName && rec.ID != model.ID);
            if (element != null)
                throw new Exception("Уже есть склад с таким названием");
            element = source.Reserves.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ReserveName = model.ReserveName;
        }

        public void deleteElement(int id)
        {
            Reserve element = source.Reserves.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                source.ReserveElements.RemoveAll(rec => rec.ReserveID == id);
                source.Reserves.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
