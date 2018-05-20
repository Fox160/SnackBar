using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnackBarService;
using SnackBarService.ImplementationsDB;
using SnackBarService.ImplementationsList;
using SnackBarService.Interfaces;

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
            APICustomer.Connect();
            MailClient.CheckMail();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
