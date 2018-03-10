using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.DataFromUser
{
    public class BoundBookingModel
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public int OutputID { get; set; }

        public int? ExecutorID { get; set; }

        public int Count { get; set; }

        public decimal Summa { get; set; }
    }
}
