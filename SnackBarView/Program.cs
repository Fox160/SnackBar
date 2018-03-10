using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnackBarService.ImplementationsList;
using SnackBarService.Interfaces;
using Unity;
using Unity.Lifetime;

namespace SnackBarView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<InterfaceCustomerService, ClientServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceComponentService, ComponentServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceExecutorService, ExecutorServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceOutputService, ProductServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceReserveService, ReserveServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<InterfaceMainService, MainServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
