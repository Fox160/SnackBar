using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelReserveView
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ReserveName { get; set; }

        [DataMember]
        public List<ModelReserveElementView> ReserveElements { get; set; }
    }
}
