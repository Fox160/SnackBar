using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelBookingView
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public string CustomerFullName { get; set; }

        [DataMember]
        public int OutputID { get; set; }

        [DataMember]
        public string OutputName { get; set; }

        [DataMember]
        public int? ExecutorID { get; set; }

        [DataMember]
        public string ExecutorName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Summa { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string DateOfCreate { get; set; }

        [DataMember]
        public string DateOfImplement { get; set; }
    }
}
