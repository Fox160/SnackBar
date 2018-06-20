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
    /// <summary>
    /// Логика взаимодействия для Reserves.xaml
    /// </summary>
    public partial class FormReserves : Window
    {
        public FormReserves()
        {
            InitializeComponent();
            Loaded += FormReserves_Load;
        }

        private void FormReserves_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APICustomer.GetRequest("api/Reserve/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ModelReserveView> list = APICustomer.GetElement<List<ModelReserveView>>(response);
                    if (list != null)
                    {
                        dataGridViewReserves.ItemsSource = list;
                        dataGridViewReserves.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewReserves.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormReserve();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedItem != null)
            {
                var form = new FormReserve();
                form.ID = ((ModelReserveView)dataGridViewReserves.SelectedItem).ID;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ModelReserveView)dataGridViewReserves.SelectedItem).ID;
                    try
                    {
                        var response = APICustomer.PostRequest("api/Reserve/DelElement", new BoundCustomerModel { ID = id });
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
