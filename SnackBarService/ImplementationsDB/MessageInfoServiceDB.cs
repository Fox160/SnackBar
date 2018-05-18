using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SnackBarService.ImplementationsDB
{
    public class MessageInfoServiceDB : InterfaceMessageInfoService
    {
        private AbstractDbContext context;

        public MessageInfoServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelMessageInfoView> GetList()
        {
            List<ModelMessageInfoView> result = context.MessageInfos
                .Where(rec => !rec.CustomerId.HasValue)
                .OrderByDescending(rec => rec.DateDelivery)
                .Select(rec => new ModelMessageInfoView
                {
                    MessageId = rec.MessageId,
                    CustomerName = rec.FromMailAddress,
                    DateDelivery = rec.DateDelivery,
                    Subject = rec.Subject,
                    Body = rec.Body
                })
                .ToList();
            return result;
        }

        public ModelMessageInfoView GetElement(int id)
        {
            MessageInfo element = context.MessageInfos.Include(rec => rec.Customer)
                .FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ModelMessageInfoView
                {
                    MessageId = element.MessageId,
                    CustomerName = element.Customer.CustomerFullName,
                    DateDelivery = element.DateDelivery,
                    Subject = element.Subject,
                    Body = element.Body
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BoundMessageInfoModel model)
        {
            MessageInfo element = context.MessageInfos.FirstOrDefault(rec => rec.MessageId == model.MessageId);
            if (element != null)
            {
                return;
            }
            var message = new MessageInfo
            {
                MessageId = model.MessageId,
                FromMailAddress = model.FromMailAddress,
                DateDelivery = model.DateDelivery,
                Subject = model.Subject,
                Body = model.Body
            };

            var mailAddress = Regex.Match(model.FromMailAddress, @"(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))");
            if (mailAddress.Success)
            {
                var client = context.Customers.FirstOrDefault(rec => rec.Mail == mailAddress.Value);
                if (client != null)
                {
                    message.CustomerId = client.ID;
                }
            }

            context.MessageInfos.Add(message);
            context.SaveChanges();
        }
    }
}
