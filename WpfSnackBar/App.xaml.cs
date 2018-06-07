using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SnackBarService.ImplementationsList;
using SnackBarService.Interfaces;
using System.Windows;
using Unity;
using Unity.Lifetime;

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
            currentContainer.RegisterType<InterfaceCustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceComponentService, ElementServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceExecutorService, ExecutorServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceOutputService, OutputServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceReserveService, ReserveServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceMainService, MainServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
