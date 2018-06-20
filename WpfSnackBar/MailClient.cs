using OpenPop.Pop3;
using SnackBarService.DataFromUser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfSnackBar
{
    public static class MailClient
    {
        public static void CheckMail()
        {
            using (Pop3Client client = new Pop3Client())
            {
                client.Connect("pop.gmail.com", 995, true);
                client.Authenticate(
                    "recent:" + ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]
                );

                int messageCount = client.GetMessageCount();
                List<OpenPop.Mime.Message> allMessages = new List<OpenPop.Mime.Message>(messageCount);
                for (int i = messageCount; i > 0; i--)
                {
                    var header = client.GetMessageHeaders(i);
                    foreach (var toEmail in header.To)
                    {
                        if (toEmail.Address.ToLower().Equals(ConfigurationManager.AppSettings["MailLogin"].ToLower()))
                        {
                            var message = client.GetMessage(i);
                            var body = message.FindFirstPlainTextVersion();
                            var body_temp = message.FindAllTextVersions();
                            APICustomer.PostRequestData("api/MessageInfo/AddElement",
                                new BoundMessageInfoModel
                                {
                                    MessageId = message.Headers.MessageId,
                                    FromMailAddress = message.Headers.From.Address,
                                    DateDelivery = message.Headers.DateSent,
                                    Subject = message.Headers.Subject,
                                    Body = body_temp.ToString()
                                }
                            );
                        }
                    }
                }
            }
        }
    }
}
