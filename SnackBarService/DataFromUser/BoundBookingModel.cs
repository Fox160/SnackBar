using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundBookingModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public int OutputID { get; set; }

        [DataMember]
        public int? ExecutorID { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Summa { get; set; }
    }
}
