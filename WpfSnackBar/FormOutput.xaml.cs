using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для FormOutput.xaml
    /// </summary>
    public partial class FormOutput : Window
    {
        public int ID { set { id = value; } }

        private int? id;

        private List<ModelProdElementView> productElements;

        public FormOutput()
        {
            InitializeComponent();
            Loaded += FormOutput_Load;
        }

        private void FormOutput_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var product = Task.Run(() => APICustomer.GetRequestData<ModelOutputView>("api/Output/Get/" + id.Value)).Result;
                    textBoxName.Text = product.OutputName;
                    textBoxPrice.Text = product.Price.ToString();
                    productElements = product.OutputElements;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            var form = new FormOutputElement();
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
                var form = new FormOutputElement();
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

            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Output/UpdElement", new BoundOutputModel
                {
                    ID = id.Value,
                    OutputName = name,
                    Price = price,
                    OutputElements = productComponentBM
                }));
            }
            else
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Output/AddElement", new BoundOutputModel
                {
                    OutputName = name,
                    Price = price,
                    OutputElements = productComponentBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
            TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
