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
    /// Логика взаимодействия для FormExecutors.xaml
    /// </summary>
    public partial class FormExecutors : Window
    {
        public FormExecutors()
        {
            InitializeComponent();
            Loaded += FormExecutors_Load;
        }

        private void FormExecutors_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APICustomer.GetRequest("api/Executor/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ModelExecutorView> list = APICustomer.GetElement<List<ModelExecutorView>>(response);
                    if (list != null)
                    {
                        dataGridViewExecutors.ItemsSource = list;
                        dataGridViewExecutors.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewExecutors.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormExecutor();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewExecutors.SelectedItem != null)
            {
                var form = new FormExecutor();
                form.ID = ((ModelExecutorView)dataGridViewExecutors.SelectedItem).ID;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewExecutors.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ModelExecutorView)dataGridViewExecutors.SelectedItem).ID;
                    try
                    {
                        var response = APICustomer.PostRequest("api/Executor/DelElement", new BoundCustomerModel { ID = id });
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
