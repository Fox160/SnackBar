using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelCustomerView
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string CustomerFullName { get; set; }
    }
}
