using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.ViewModel
{
    public class ModelCustomerBookingsView
    {
        public string CustomerName { get; set; }

        public string DateOfCreate { get; set; }

        public string OutputName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }
    }
}
