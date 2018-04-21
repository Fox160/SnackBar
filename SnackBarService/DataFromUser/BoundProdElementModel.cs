using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SnackBarService.DataFromUser
{
    [DataContract]
    public class BoundProdElementModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int OutputID { get; set; }

        [DataMember]
        public int ElementID { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
