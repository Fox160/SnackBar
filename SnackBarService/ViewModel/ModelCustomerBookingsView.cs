using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelCustomerBookingsView
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string DateOfCreate { get; set; }

        [DataMember]
        public string OutputName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
