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
    /// Логика взаимодействия для FormPutOnReserve.xaml
    /// </summary>
    public partial class FormPutOnReserve : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceReserveService serviceReserve;

        private readonly InterfaceComponentService serviceComponent;

        private readonly InterfaceMainService serviceMain;

        public FormPutOnReserve(InterfaceReserveService serviceS, InterfaceComponentService serviceC, InterfaceMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormPutOnReserve_Load;
            this.serviceReserve = serviceS;
            this.serviceComponent = serviceC;
            this.serviceMain = serviceM;
        }

        private void FormPutOnReserve_Load(object sender, EventArgs e)
        {
            try
            {
                List<ModelElementView> listElement = serviceComponent.getList();
                if (listElement != null)
                {
                    comboBoxComponent.DisplayMemberPath = "ElementName";
                    comboBoxComponent.SelectedValuePath = "Id";
                    comboBoxComponent.ItemsSource = listElement;
                    comboBoxComponent.SelectedItem = null;
                }
                List<ModelReserveView> listReserve = serviceReserve.getList();
                if (listReserve != null)
                {
                    comboBoxStock.DisplayMemberPath = "ReserveName";
                    comboBoxStock.SelectedValuePath = "Id";
                    comboBoxStock.ItemsSource = listReserve;
                    comboBoxStock.SelectedItem = null;
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
                serviceMain.putComponentOnReserve(new BoundResElementModel
                {
                    ElementID = ((ModelElementView) comboBoxComponent.SelectedItem).ID,
                    ReserveID = ((ModelReserveView) comboBoxStock.SelectedItem).ID,
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
