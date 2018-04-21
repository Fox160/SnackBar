using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelReservesLoadView
    {
        [DataMember]
        public string ReserveName { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public List<StocksComponentLoadViewModel> Elements { get; set; }
    }

    [DataContract]
    public class StocksComponentLoadViewModel
    {
        [DataMember]
        public string ElementName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
