using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnackBarModel;

namespace SnackBarModel
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Clients { get; set; }

        public List<Element> Elements { get; set; }

        public List<Executor> Executors { get; set; }

        public List<Booking> Orders { get; set; }

        public List<Output> Products { get; set; }

        public List<OutputElement> ProductElements { get; set; }

        public List<Reserve> Reserves { get; set; }

        public List<ReserveElement> ReserveElements { get; set; }

        private DataListSingleton()
        {
            Clients = new List<Customer>();
            Elements = new List<Element>();
            Executors = new List<Executor>();
            Orders = new List<Booking>();
            Products = new List<Output>();
            ProductElements = new List<OutputElement>();
            Reserves = new List<Reserve>();
            ReserveElements = new List<ReserveElement>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
                instance = new DataListSingleton();

            return instance;
        }
    }
}
