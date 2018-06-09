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
using Unity;
using Unity.Attributes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormCreateBooking.xaml
    /// </summary>
    public partial class FormCreateBooking : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceCustomerService serviceClient;

        private readonly InterfaceOutputService serviceProduct;

        private readonly InterfaceMainService serviceMain;


        public FormCreateBooking(InterfaceCustomerService serviceC, InterfaceOutputService serviceP, InterfaceMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormCreateBooking_Load;
            comboBoxProduct.SelectionChanged += comboBoxProduct_SelectedIndexChanged;

            comboBoxProduct.SelectionChanged += new SelectionChangedEventHandler(comboBoxProduct_SelectedIndexChanged);
            this.serviceClient = serviceC;
            this.serviceProduct = serviceP;
            this.serviceMain = serviceM;
        }

        private void FormCreateBooking_Load(object sender, EventArgs e)
        {
            try
            {
                List<ModelCustomerView> listClient = serviceClient.getList();
                if (listClient != null)
                {
                    comboBoxClient.DisplayMemberPath = "CustomerFullName";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listClient;
                    comboBoxProduct.SelectedItem = null;
                }
                List<ModelOutputView> listProduct = serviceProduct.getList();
                if (listProduct != null)
                {
                    comboBoxProduct.DisplayMemberPath = "OutputName";
                    comboBoxProduct.SelectedValuePath = "Id";
                    comboBoxProduct.ItemsSource = listProduct;
                    comboBoxProduct.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxProduct.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((ModelOutputView)comboBoxProduct.SelectedItem).ID;
                    ModelOutputView product = serviceProduct.getElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = Convert.ToInt32(count * product.Price).ToString();
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
                serviceMain.createOrder(new BoundBookingModel
                {
                    CustomerID = Convert.ToInt32(((ModelCustomerView)comboBoxClient.SelectedItem).ID),
                    OutputID = Convert.ToInt32(((ModelOutputView)comboBoxProduct.SelectedItem).ID),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Summa = Convert.ToInt32(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
