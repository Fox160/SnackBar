using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.DataFromUser
{
    public class BoundOutputModel
    {
        public int ID { get; set; }

        public string OutputName { get; set; }

        public decimal Price { get; set; }

        public List<BoundProdElementModel> OutputElements { get; set; }
    }
}
