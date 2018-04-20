using System.Runtime.Serialization;

namespace SnackBarService.ViewModel
{
    [DataContract]
    public class ModelExecutorView
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ExecutorFullName { get; set; }
    }
}
