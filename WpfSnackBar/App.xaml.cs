using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SnackBarService;
using SnackBarService.ImplementationsDB;
using SnackBarService.Interfaces;
using System.Windows;
using Unity;
using Unity.Lifetime;
using System.Data.Entity;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<InterfaceCustomerService, CustomerServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceComponentService, ElementServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceExecutorService, ExecutorServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceOutputService, OutputServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceReserveService, ReserveServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceReportService, ReportServiceDB>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
