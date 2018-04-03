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
using Unity;
using Unity.Attributes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormExecutors.xaml
    /// </summary>
    public partial class FormExecutors : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceExecutorService service;

        public FormExecutors(InterfaceExecutorService service)
        {
            InitializeComponent();
            Loaded += FormExecutors_Load;
            this.service = service;
        }

        private void FormExecutors_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ModelExecutorView> list = service.getList();
                if (list != null)
                {
                    dataGridViewExecutors.ItemsSource = list;
                    dataGridViewExecutors.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewExecutors.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormExecutor>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewExecutors.SelectedItem != null)
            {
                var form = Container.Resolve<FormExecutor>();
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
                        service.deleteElement(id);
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
