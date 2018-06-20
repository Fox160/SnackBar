using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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
    // <summary>
    // Логика взаимодействия для FormOutputs.xaml
    // </summary>
    public partial class FormOutputs : Window
    {
        public FormOutputs()
        {
            InitializeComponent();
            Loaded += FormOutputs_Load;
        }

        private void FormOutputs_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APICustomer.GetRequest("api/Output/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ModelOutputView> list = APICustomer.GetElement<List<ModelOutputView>>(response);
                    if (list != null)
                    {
                        dataGridViewProducts.ItemsSource = list;
                        dataGridViewProducts.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewProducts.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewProducts.Columns[3].Visibility = Visibility.Hidden;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormOutput();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedItem != null)
            {
                var form = new FormOutput();
                form.ID = ((ModelOutputView)dataGridViewProducts.SelectedItem).ID;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((ModelOutputView)dataGridViewProducts.SelectedItem).ID;
                    try
                    {
                        var response = APICustomer.PostRequest("api/Output/DelElement", new BoundCustomerModel { ID = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APICustomer.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
