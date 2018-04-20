using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundOutputModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string OutputName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<BoundProdElementModel> OutputElements { get; set; }
    }
}
