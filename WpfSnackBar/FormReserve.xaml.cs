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
using Unity;
using Unity.Attributes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для Reserve.xaml
    /// </summary>
    public partial class FormReserve : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceReserveService service;

        private int? id;

        public FormReserve(InterfaceReserveService service)
        {
            InitializeComponent();
            Loaded += FormReserve_Load;
            this.service = service;
        }

        private void FormReserve_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ModelReserveView view = service.getElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ReserveName;
                        dataGridViewReserve.ItemsSource = view.ReserveElements;
                        dataGridViewReserve.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewReserve.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.updateElement(new BoundReserveModel
                    {
                        ID = id.Value,
                        ReserveName = textBoxName.Text
                    });
                }
                else
                {
                    service.addElement(new BoundReserveModel
                    {
                        ReserveName = textBoxName.Text
                    });
                }
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
