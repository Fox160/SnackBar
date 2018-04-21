using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelElementView
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ElementName { get; set; }
    }
}
