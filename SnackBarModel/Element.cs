using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackBarModel
{
    public class Element
    {
        public int ID { get; set; }

        [Required]
        public string ElementName { get; set; }

        [ForeignKey("ElementID")]
        public virtual List<OutputElement> OutputElements { get; set; }

        [ForeignKey("ElementID")]
        public virtual List<ReserveElement> ReserveElements { get; set; }
    }
}
