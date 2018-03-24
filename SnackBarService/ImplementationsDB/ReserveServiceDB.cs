using System;
using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.ImplementationsDB
{
    public class ReserveServiceDB : InterfaceReserveService
    {
        private AbstractDbContext context;

        public ReserveServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelReserveView> getList()
        {
            List<ModelReserveView> result = context.Reserves
                .Select(rec => new ModelReserveView
                {
                    ID = rec.ID,
                    ReserveName = rec.ReserveName,
                    ReserveElements = context.ReserveElements
                            .Where(recPC => recPC.ReserveID == rec.ID)
                            .Select(recPC => new ModelReserveElementView
                            {
                                ID = recPC.ID,
                                ReserveID = recPC.ReserveID,
                                ElementID = recPC.ElementID,
                                ElementName = recPC.Element.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ModelReserveView getElement(int id)
        {
            Reserve element = context.Reserves.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelReserveView
                {
                    ID = element.ID,
                    ReserveName = element.ReserveName,
                    ReserveElements = context.ReserveElements
                            .Where(recPC => recPC.ReserveID == element.ID)
                            .Select(recPC => new ModelReserveElementView
                            {
                                ID = recPC.ID,
                                ReserveID = recPC.ReserveID,
                                ElementID = recPC.ElementID,
                                ElementName = recPC.Element.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundReserveModel model)
        {
            Reserve element = context.Reserves.FirstOrDefault(rec => rec.ReserveName == model.ReserveName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Reserves.Add(new Reserve
            {
                ReserveName = model.ReserveName
            });
            context.SaveChanges();
        }

        public void updateElement(BoundReserveModel model)
        {
            Reserve element = context.Reserves.FirstOrDefault(rec =>
                                        rec.ReserveName == model.ReserveName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Reserves.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ReserveName = model.ReserveName;
            context.SaveChanges();
        }

        public void deleteElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Reserve element = context.Reserves.FirstOrDefault(rec => rec.ID == id);
                    if (element != null)
                    {
                        context.ReserveElements.RemoveRange(
                                            context.ReserveElements.Where(rec => rec.ReserveID == id));
                        context.Reserves.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
