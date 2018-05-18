using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundMessageInfoModel
    {
        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        public string FromMailAddress { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public DateTime DateDelivery { get; set; }
    }
}
