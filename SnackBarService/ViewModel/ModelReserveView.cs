using System.Collections.Generic;

namespace SnackBarService.ViewModel
{
    public class ModelReserveView
    {
        public int ID { get; set; }

        public string ReserveName { get; set; }

        public List<ModelReserveComponentView> ReserveElements { get; set; }
    }
}
