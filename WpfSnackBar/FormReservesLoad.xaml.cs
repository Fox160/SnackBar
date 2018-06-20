using Microsoft.Win32;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormReservesLoad.xaml
    /// </summary>
    public partial class FormReservesLoad : Window
    {
        public FormReservesLoad()
        {
            InitializeComponent();
        }

        private void FormReservesLoad_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView.Items.Clear();
                foreach (var elem in Task.Run(() => APICustomer.GetRequestData<List<ModelReservesLoadView>>("api/Report/GetReservesLoad")).Result)
                {
                    dataGridView.Items.Add(new object[] { elem.ReserveName, "", "" });
                    foreach (var listElem in elem.Elements)
                    {
                        dataGridView.Items.Add(new object[] { "", listElem.ElementName, listElem.Count });
                    }
                    dataGridView.Items.Add(new object[] { "Итого", "", elem.TotalCount });
                    dataGridView.Items.Add(new object[] { });
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APICustomer.PostRequestData("api/Report/SaveReservesLoad", new BoundReportModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
