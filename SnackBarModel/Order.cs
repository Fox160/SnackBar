using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarModel
{
    public class Order
    {
        public int ID { get; set; }

        public int ClientID { get; set; }

        public int ProductID { get; set; }

        public int? ExecutorID { get; set; }

        public int Count { get; set; }

        public decimal Summa { get; set; }

        public StatusOfOrder Status { get; set; }

        public DateTime DateOfCreate { get; set; }

        public DateTime? DateOfImplement { get; set; }
    }
}
