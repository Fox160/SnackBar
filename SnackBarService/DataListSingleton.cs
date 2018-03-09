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

        public List<Customer> Customers { get; set; }

        public List<Element> Elements { get; set; }

        public List<Executor> Executors { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<Output> Outputs { get; set; }

        public List<OutputElement> OutputElements { get; set; }

        public List<Reserve> Reserves { get; set; }

        public List<ReserveElement> ReserveElements { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Elements = new List<Element>();
            Executors = new List<Executor>();
            Bookings = new List<Booking>();
            Outputs = new List<Output>();
            OutputElements = new List<OutputElement>();
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
