using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarModel
{
    public class Booking
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public int OutputID { get; set; }

        public int? ExecutorID { get; set; }

        public int Count { get; set; }

        public decimal Summa { get; set; }

        public StatusOfBooking Status { get; set; }

        public DateTime DateOfCreate { get; set; }

        public DateTime? DateOfImplement { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Output Output { get; set; }

        public virtual Executor Executor { get; set; }
    }
}
