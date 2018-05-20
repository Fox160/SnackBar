using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundCustomerModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public string CustomerFullName { get; set; }
    }
}
