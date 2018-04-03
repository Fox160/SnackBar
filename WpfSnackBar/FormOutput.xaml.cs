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
using Unity;
using Unity.Attributes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormOutput.xaml
    /// </summary>
    public partial class FormOutput : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceOutputService service;

        private int? id;

        private List<ModelProdElementView> productElements;

        public FormOutput(InterfaceOutputService service)
        {
            InitializeComponent();
            Loaded += FormOutput_Load;
            this.service = service;
        }

        private void FormOutput_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ModelOutputView view = service.getElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.OutputName;
                        textBoxPrice.Text = view.Price.ToString();
                        productElements = view.OutputElements;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                productElements = new List<ModelProdElementView>();
        }

        private void LoadData()
        {
            try
            {
                if (productElements != null)
                {
                    dataGridViewProduct.ItemsSource = null;
                    dataGridViewProduct.ItemsSource = productElements;
                    dataGridViewProduct.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormOutputElement>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.OutputID = id.Value;
                    productElements.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                var form = Container.Resolve<FormOutputElement>();
                form.Model = productElements[dataGridViewProduct.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    productElements[dataGridViewProduct.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        productElements.RemoveAt(dataGridViewProduct.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (productElements == null || productElements.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<BoundProdElementModel> productComponentBM = new List<BoundProdElementModel>();
                for (int i = 0; i < productElements.Count; ++i)
                {
                    productComponentBM.Add(new BoundProdElementModel
                    {
                        ID = productElements[i].ID,
                        OutputID = productElements[i].OutputID,
                        ElementID = productElements[i].ElementID,
                        Count = productElements[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.updateElement(new BoundOutputModel
                    {
                        ID = id.Value,
                        OutputName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        OutputElements = productComponentBM
                    });
                }
                else
                {
                    service.addElement(new BoundOutputModel
                    {
                        OutputName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        OutputElements = productComponentBM
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
