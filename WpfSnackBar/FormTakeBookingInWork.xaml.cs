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
    /// Логика взаимодействия для TakeBookingInWork.xaml
    /// </summary>
    public partial class FormTakeBookingInWork : Window
    {
        public int ID { set { id = value; } }

        private int? id;

        public FormTakeBookingInWork()
        {
            InitializeComponent();
            Loaded += FormTakeBookingInWork_Load;
        }

        private void FormTakeBookingInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                var response = APICustomer.GetRequest("api/Executor/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ModelExecutorView> list = APICustomer.GetElement<List<ModelExecutorView>>(response);
                    if (list != null)
                    {
                        comboBoxExecutor.DisplayMemberPath = "ExecutorFullName";
                        comboBoxExecutor.SelectedValuePath = "Id";
                        comboBoxExecutor.ItemsSource = list;
                        comboBoxExecutor.SelectedItem = null;
                    }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxExecutor.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APICustomer.PostRequest("api/Main/TakeBookingInWork", new BoundBookingModel
                {
                    ID = id.Value,
                    ExecutorID = ((ModelExecutorView)comboBoxExecutor.SelectedItem).ID
                });
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
