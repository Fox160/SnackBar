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
    /// Логика взаимодействия для TakeBookingInWork.xaml
    /// </summary>
    public partial class FormTakeBookingInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceExecutorService serviceExecutor;

        private readonly InterfaceMainService serviceMain;

        private int? id;

        public FormTakeBookingInWork(InterfaceExecutorService serviceI, InterfaceMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormTakeBookingInWork_Load;
            this.serviceExecutor = serviceI;
            this.serviceMain = serviceM;
        }

        private void FormTakeBookingInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<ModelExecutorView> listExecutor = serviceExecutor.getList();
                if (listExecutor != null)
                {
                    comboBoxExecutor.DisplayMemberPath = "ExecutorFullName";
                    comboBoxExecutor.SelectedValuePath = "Id";
                    comboBoxExecutor.ItemsSource = listExecutor;
                    comboBoxExecutor.SelectedItem = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxExecutor.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.takeOrderInWork(new BoundBookingModel
                {
                    ID = id.Value,
                    ExecutorID = ((ModelExecutorView)comboBoxExecutor.SelectedItem).ID,
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
