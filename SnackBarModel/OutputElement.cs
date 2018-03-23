using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarModel
{
    public class OutputElement
    {
        public int ID { get; set; }

        public int OutputID { get; set; }

        public int ElementID { get; set; }

        public int Count { get; set; }

        public virtual Output Output { get; set; }

        public virtual Element Element { get; set; }
    }
}
