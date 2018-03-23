using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarModel
{
    public class Customer
    {
        public int ID { get; set; }

        [Required]
        public string CustomerFullName { get; set; }

        [ForeignKey("CustomerID")]
        public virtual List<Booking> Bookings { get; set; }
    }
}
