using System.Collections.Generic;

namespace SnackBarService.ViewModel
{
    public class ModelProductView
    {
        public int ID { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public List<ModelProdElementView> ProductElements { get; set; }
    }
}
