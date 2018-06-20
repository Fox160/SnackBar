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
            APICustomer.Connect();
            MailClient.CheckMail();
            var application = new App();
            application.Run(new FormMain());
        }
    }
}
