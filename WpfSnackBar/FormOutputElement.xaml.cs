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
    /// Логика взаимодействия для FormOutputElement.xaml
    /// </summary>
    public partial class FormOutputElement : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public ModelProdElementView Model { set { model = value; } get { return model; } }

        private readonly InterfaceComponentService service;

        private ModelProdElementView model;

        public FormOutputElement(InterfaceComponentService service)
        {
            InitializeComponent();
            Loaded += FormOutputElement_Load;
            this.service = service;
        }

        private void FormOutputElement_Load(object sender, EventArgs e)
        {
            List<ModelElementView> list = service.getList();
            try
            {
                if (list != null)
                {
                    comboBoxComponent.DisplayMemberPath = "ElementName";
                    comboBoxComponent.SelectedValuePath = "Id";
                    comboBoxComponent.ItemsSource = list;
                    comboBoxComponent.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxComponent.IsEnabled = false;
                comboBoxComponent.SelectedItem = model;
                textBoxCount.Text = model.Count.ToString();
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
            try
            {
                if (model == null)
                {
                    model = new ModelProdElementView
                    {
                        ElementID = ((ModelElementView) comboBoxComponent.SelectedItem).ID,
                        ElementName = comboBoxComponent.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
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
