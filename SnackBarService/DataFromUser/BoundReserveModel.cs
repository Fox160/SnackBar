using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundReserveModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ReserveName { get; set; }
    }
}
