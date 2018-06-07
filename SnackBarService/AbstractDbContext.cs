using System;
using System.Collections.Generic;
using SnackBarModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService
{
    [Table("AbstractDatabase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Element> Elements { get; set; }

        public virtual DbSet<Executor> Executors { get; set; }

        public virtual DbSet<Booking> Bookings { get; set; }

        public virtual DbSet<Output> Outputs { get; set; }

        public virtual DbSet<OutputElement> OutputElements { get; set; }

        public virtual DbSet<Reserve> Reserves { get; set; }

        public virtual DbSet<ReserveElement> ReserveElements { get; set; }
    }
}
