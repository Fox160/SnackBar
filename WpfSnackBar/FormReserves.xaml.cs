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
    /// Логика взаимодействия для Reserves.xaml
    /// </summary>
    public partial class FormReserves : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceReserveService service;

        public FormReserves(InterfaceReserveService service)
        {
            InitializeComponent();
            Loaded += FormReserves_Load;
            this.service = service;
        }

        private void FormReserves_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ModelReserveView> list = service.getList();
                if (list != null)
                {
                    dataGridViewReserves.ItemsSource = list;
                    dataGridViewReserves.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewReserves.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReserve>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedItem != null)
            {
                var form = Container.Resolve<FormReserve>();
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
