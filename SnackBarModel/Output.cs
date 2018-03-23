using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarModel
{
    public class Output
    {
        public int ID { get; set; }

        [Required]
        public string OutputName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("OutputID")]
        public virtual List<Booking> Bookings { get; set; }

        [ForeignKey("OutputID")]
        public virtual List<OutputElement> OutputElements { get; set; }
    }
}
