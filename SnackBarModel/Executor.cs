using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackBarModel
{
    public class Executor
    {
        public int ID { get; set; }

        [Required]
        public string ExecutorFullName { get; set; }

        [ForeignKey("ExecutorID")]
        public virtual List<Booking> Bookings { get; set; }
    }
}
