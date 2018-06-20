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
    /// Логика взаимодействия для FormPutOnReserve.xaml
    /// </summary>
    public partial class FormPutOnReserve : Window
    {
        public FormPutOnReserve()
        {
            InitializeComponent();
            Loaded += FormPutOnReserve_Load;
        }

        private void FormPutOnReserve_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APICustomer.GetRequest("api/Element/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<ModelElementView> list = APICustomer.GetElement<List<ModelElementView>>(responseC);
                    if (list != null)
                    {
                        comboBoxComponent.DisplayMemberPath = "ElementName";
                        comboBoxComponent.SelectedValuePath = "Id";
                        comboBoxComponent.ItemsSource = list;
                        comboBoxComponent.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseC));
                }
                var responseS = APICustomer.GetRequest("api/Reserve/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<ModelReserveView> list = APICustomer.GetElement<List<ModelReserveView>>(responseS);
                    if (list != null)
                    {
                        comboBoxStock.DisplayMemberPath = "ReserveName";
                        comboBoxStock.SelectedValuePath = "Id";
                        comboBoxStock.ItemsSource = list;
                        comboBoxStock.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxComponent.SelectedItem == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxStock.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APICustomer.PostRequest("api/Main/PutElementOnReserve", new BoundResElementModel
                {
                    ElementID = ((ModelElementView)comboBoxComponent.SelectedItem).ID,
                    ReserveID = ((ModelReserveView)comboBoxStock.SelectedItem).ID,
                    Count = Convert.ToInt32(textBoxCount.Text)
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
