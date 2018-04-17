using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.ViewModel
{
    public class ModelReservesLoadView
    {
        public string ReserveName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Elements { get; set; }
    }
}
