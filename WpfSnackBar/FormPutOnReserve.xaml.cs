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

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnReserve.xaml
    /// </summary>
    public partial class FormPutOnReserve : Window
    {
        public FormPutOnReserve()
        {
            InitializeComponent();
            Loaded += FormPutOnReserve_Load;
        }

        private void FormPutOnReserve_Load(object sender, EventArgs e)
        {
            try
            {
                List<ModelElementView> list = Task.Run(() => APICustomer.GetRequestData<List<ModelElementView>>("api/Element/GetList")).Result;
                if (list != null)
                {
                    comboBoxComponent.DisplayMemberPath = "ElementName";
                    comboBoxComponent.SelectedValuePath = "Id";
                    comboBoxComponent.ItemsSource = list;
                    comboBoxComponent.SelectedItem = null;
                }


                List<ModelReserveView> listR = Task.Run(() => APICustomer.GetRequestData<List<ModelReserveView>>("api/Reserve/GetList")).Result;
                if (list != null)
                {
                    comboBoxStock.DisplayMemberPath = "ReserveName";
                    comboBoxStock.SelectedValuePath = "Id";
                    comboBoxStock.ItemsSource = listR;
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
                int elementId = ((ModelElementView)comboBoxComponent.SelectedItem).ID;
                int reserveId = ((ModelReserveView)comboBoxStock.SelectedItem).ID;
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APICustomer.PostRequestData("api/Main/PutElementOnReserve", new BoundResElementModel
                {
                    ElementID = elementId,
                    ReserveID = reserveId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Склад пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
