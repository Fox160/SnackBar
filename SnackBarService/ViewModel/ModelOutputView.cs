using System.Collections.Generic;

namespace SnackBarService.ViewModel
{
    public class ModelOutputView
    {
        public int ID { get; set; }

        public string OutputName { get; set; }

        public decimal Price { get; set; }

        public List<ModelProdElementView> OutputElements { get; set; }
    }
}
