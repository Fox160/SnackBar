using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundExecutorModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ExecutorFullName { get; set; }
    }
}
