namespace SnackBarService.ViewModel
{
    public class OrderViewModel
    {
        public int ID { get; set; }

        public int ClientID { get; set; }

        public string ClientFullName { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int? ExecutorID { get; set; }

        public string ExecutorName { get; set; }

        public int Count { get; set; }

        public decimal Summa { get; set; }

        public string Status { get; set; }

        public string DateOfCreate { get; set; }

        public string DateOfImplement { get; set; }
    }
}
