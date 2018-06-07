using System;
using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace SnackBarService.ImplementationsDB
{
    public class MainServiceBD : InterfaceMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelBookingView> getList()
        {
            List<ModelBookingView> result = context.Bookings
                .Select(rec => new ModelBookingView
                {
                    ID = rec.ID,
                    CustomerID = rec.CustomerID,
                    OutputID = rec.OutputID,
                    ExecutorID = rec.ExecutorID,
                    DateOfCreate = SqlFunctions.DateName("dd", rec.DateOfCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateOfCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateOfCreate),
                    DateOfImplement = rec.DateOfImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateOfImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateOfImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateOfImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Summa = rec.Summa,
                    CustomerFullName = rec.Customer.CustomerFullName,
                    OutputName = rec.Output.OutputName,
                    ExecutorName = rec.Executor.ExecutorFullName
                })
                .ToList();
            return result;
        }

        public void createOrder(BoundBookingModel model)
        {
            context.Bookings.Add(new Booking
            {
                CustomerID = model.CustomerID,
                OutputID = model.OutputID,
                DateOfCreate = DateTime.Now,
                Count = model.Count,
                Summa = model.Summa,
                Status = StatusOfBooking.Принятый
            });
            context.SaveChanges();
        }

        public void takeOrderInWork(BoundBookingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Booking element = context.Bookings.FirstOrDefault(rec => rec.ID == model.ID);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var outputElements = context.OutputElements
                                                .Include(rec => rec.Element)
                                                .Where(rec => rec.OutputID == element.OutputID);
                    foreach (var productComponent in outputElements)
                    {
                        int countOnStocks = productComponent.Count * element.Count;
                        var stockComponents = context.ReserveElements
                                                    .Where(rec => rec.ElementID == productComponent.ElementID);
                        foreach (var stockComponent in stockComponents)
                        {
                            if (stockComponent.Count >= countOnStocks)
                            {
                                stockComponent.Count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= stockComponent.Count;
                                stockComponent.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                productComponent.Element.ElementName + " требуется " +
                                productComponent.Count + ", не хватает " + countOnStocks);
                        }
                    }
                    element.ExecutorID = model.ExecutorID;
                    element.DateOfImplement = DateTime.Now;
                    element.Status = StatusOfBooking.Выполняемый;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void finishOrder(int id)
        {
            Booking element = context.Bookings.FirstOrDefault(rec => rec.ID == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = StatusOfBooking.Готов;
            context.SaveChanges();
        }

        public void payOrder(int id)
        {
            Booking element = context.Bookings.FirstOrDefault(rec => rec.ID == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = StatusOfBooking.Оплаченный;
            context.SaveChanges();
        }

        public void putComponentOnReserve(BoundResElementModel model)
        {
            ReserveElement element = context.ReserveElements
                                                .FirstOrDefault(rec => rec.ReserveID == model.ReserveID &&
                                                                    rec.ElementID == model.ElementID);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.ReserveElements.Add(new ReserveElement
                {
                    ReserveID = model.ReserveID,
                    ElementID = model.ElementID,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }
    }
}
