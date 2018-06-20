using System;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
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
    /// Логика взаимодействия для FormCreateBooking.xaml
    /// </summary>
    public partial class FormCreateBooking : Window
    {
        public FormCreateBooking()
        {
            InitializeComponent();
            Loaded += FormCreateBooking_Load;
            comboBoxProduct.SelectionChanged += comboBoxProduct_SelectedIndexChanged;

            comboBoxProduct.SelectionChanged += new SelectionChangedEventHandler(comboBoxProduct_SelectedIndexChanged);
        }

        private void FormCreateBooking_Load(object sender, EventArgs e)
        {
            try
            {
                List<ModelCustomerView> list = Task.Run(() => APICustomer.GetRequestData<List<ModelCustomerView>>("api/Customer/GetList")).Result;
                if (list != null)
                {
                    comboBoxClient.DisplayMemberPath = "CustomerFullName";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = list;
                    comboBoxClient.SelectedItem = null;
                }

                List<ModelOutputView> listP = Task.Run(() => APICustomer.GetRequestData<List<ModelOutputView>>("api/Output/GetList")).Result;
                if (list != null)
                {
                    comboBoxProduct.DisplayMemberPath = "OutputName";
                    comboBoxProduct.SelectedValuePath = "Id";
                    comboBoxProduct.ItemsSource = listP;
                    comboBoxProduct.SelectedItem = null;
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

        private void CalcSum()
        {
            if (!string.IsNullOrEmpty(comboBoxProduct.Text) && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((ModelOutputView)comboBoxProduct.SelectedItem).ID;
                    ModelOutputView product = Task.Run(() => APICustomer.GetRequestData<ModelOutputView>("api/Output/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = Convert.ToInt32(count * product.Price).ToString();

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

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int clientId = Convert.ToInt32(((ModelCustomerView)comboBoxClient.SelectedItem).ID);
            int outputId = Convert.ToInt32(((ModelOutputView)comboBoxProduct.SelectedItem).ID);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APICustomer.PostRequestData("api/Main/CreateBooking", new BoundBookingModel
            {
                CustomerID = clientId,
                OutputID = outputId,
                Count = count,
                Summa = sum
            }));

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
