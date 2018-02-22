using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.DataFromUser
{
    public class BoundProductModel
    {
        public int ID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public List<BoundProdComponentModel> ProductElements { get; set; }
    }
}
