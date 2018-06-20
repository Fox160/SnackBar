using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для Reserve.xaml
    /// </summary>
    public partial class FormReserve : Window
    {
        public int ID { set { id = value; } }

        private int? id;

        public FormReserve()
        {
            InitializeComponent();
            Loaded += FormReserve_Load;
        }

        private void FormReserve_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var stock = Task.Run(() => APICustomer.GetRequestData<ModelReserveView>("api/Reserve/Get/" + id.Value)).Result;
                    textBoxName.Text = stock.ReserveName;
                    dataGridViewReserve.ItemsSource = stock.ReserveElements;
                    dataGridViewReserve.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewReserve.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewReserve.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewReserve.Columns[3].Width = DataGridLength.Auto;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Reserve/UpdElement", new BoundReserveModel
                {
                    ID = id.Value,
                    ReserveName = name
                }));
            }
            else
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Reserve/AddElement", new BoundReserveModel
                {
                    ReserveName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
