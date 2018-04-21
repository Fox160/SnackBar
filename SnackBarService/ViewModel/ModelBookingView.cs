namespace SnackBarService.ViewModel
{
    public class ModelBookingView
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public string CustomerFullName { get; set; }

        public int OutputID { get; set; }

        public string OutputName { get; set; }

        public int? ExecutorID { get; set; }

        public string ExecutorName { get; set; }

        public int Count { get; set; }

        public decimal Summa { get; set; }

        public string Status { get; set; }

        public string DateOfCreate { get; set; }

        public string DateOfImplement { get; set; }
    }
}
