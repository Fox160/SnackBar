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
                    var response = APICustomer.GetRequest("api/Reserve/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var stock = APICustomer.GetElement<ModelReserveView>(response);
                        textBoxName.Text = stock.ReserveName;
                        dataGridViewReserve.ItemsSource = stock.ReserveElements;
                        dataGridViewReserve.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[3].Width = DataGridLength.Auto;
                    }
                    else
                    {
                        throw new Exception(APICustomer.GetError(response));
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APICustomer.PostRequest("api/Reserve/UpdElement", new BoundReserveModel
                    {
                        ID = id.Value,
                        ReserveName = textBoxName.Text
                    });
                }
                else
                {
                    response = APICustomer.PostRequest("api/Reserve/AddElement", new BoundReserveModel
                    {
                        ReserveName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APICustomer.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
