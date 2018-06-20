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
                var responseC = APICustomer.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<ModelCustomerView> list = APICustomer.GetElement<List<ModelCustomerView>>(responseC);
                    if (list != null)
                    {
                        comboBoxClient.DisplayMemberPath = "CustomerFullName";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = list;
                        comboBoxClient.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseC));
                }
                var responseP = APICustomer.GetRequest("api/Output/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<ModelOutputView> list = APICustomer.GetElement<List<ModelOutputView>>(responseP);
                    if (list != null)
                    {
                        comboBoxProduct.DisplayMemberPath = "OutputName";
                        comboBoxProduct.SelectedValuePath = "Id";
                        comboBoxProduct.ItemsSource = list;
                        comboBoxProduct.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
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
                    var responseP = APICustomer.GetRequest("api/Output/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        ModelOutputView product = APICustomer.GetElement<ModelOutputView>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = Convert.ToInt32(count * product.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APICustomer.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                var response = APICustomer.PostRequest("api/Main/CreateBooking", new ModelBookingView
                {
                    CustomerID = Convert.ToInt32(((ModelCustomerView)comboBoxClient.SelectedItem).ID),
                    OutputID = Convert.ToInt32(((ModelOutputView)comboBoxProduct.SelectedItem).ID),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Summa = Convert.ToInt32(textBoxSum.Text)
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
