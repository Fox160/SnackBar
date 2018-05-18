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
using System.Net.Mail;
using System.Configuration;
using System.Net;

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
            var order = new Booking
            {
                CustomerID = model.CustomerID,
                OutputID = model.OutputID,
                DateOfCreate = DateTime.Now,
                Count = model.Count,
                Summa = model.Summa,
                Status = StatusOfBooking.Принятый
            };
            context.Bookings.Add(order);
            context.SaveChanges();

            var client = context.Customers.FirstOrDefault(x => x.ID == model.CustomerID);
            SendEmail(client.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} создан успешно", order.ID,
                order.DateOfCreate.ToShortDateString()));
        }

        public void takeOrderInWork(BoundBookingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Booking element = context.Bookings.Include(rec => rec.Customer).FirstOrDefault(rec => rec.ID == model.ID);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var productComponents = context.OutputElements
                                                .Include(rec => rec.Element)
                                                .Where(rec => rec.OutputID == element.OutputID);
                    // списываем
                    foreach (var productComponent in productComponents)
                    {
                        int countOnStocks = productComponent.Count * element.Count;
                        var stockComponents = context.ReserveElements
                                                    .Where(rec => rec.ElementID == productComponent.ElementID);
                        foreach (var stockComponent in stockComponents)
                        {
                            // компонентов на одном слкаде может не хватать
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
                    Customer customer = context.Customers.FirstOrDefault(x => x.ID == element.CustomerID);
                    SendEmail(customer.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передеан в работу", element.ID, element.DateOfCreate.ToShortDateString()));
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
            Customer customer = context.Customers.FirstOrDefault(x => x.ID == element.CustomerID);
            SendEmail(customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} передан на оплату", element.ID,
                element.DateOfCreate.ToShortDateString()));
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
            Customer customer = context.Customers.FirstOrDefault(x => x.ID == element.CustomerID);
            SendEmail(customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.ID, element.DateOfCreate.ToShortDateString()));
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

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
