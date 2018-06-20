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
    /// Логика взаимодействия для TakeBookingInWork.xaml
    /// </summary>
    public partial class FormTakeBookingInWork : Window
    {
        public int ID { set { id = value; } }

        private int? id;

        public FormTakeBookingInWork()
        {
            InitializeComponent();
            Loaded += FormTakeBookingInWork_Load;
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

                List<ModelExecutorView> list = Task.Run(() => APICustomer.GetRequestData<List<ModelExecutorView>>("api/Executor/GetList")).Result;
                if (list != null)
                {
                    comboBoxExecutor.DisplayMemberPath = "ExecutorFullName";
                    comboBoxExecutor.SelectedValuePath = "Id";
                    comboBoxExecutor.ItemsSource = list;
                    comboBoxExecutor.SelectedItem = null;
                }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxExecutor.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int executorId = ((ModelExecutorView)comboBoxExecutor.SelectedItem).ID;
                Task task = Task.Run(() => APICustomer.PostRequestData("api/Main/TakeBookingInWork", new BoundBookingModel
                {
                    ID = id.Value,
                    ExecutorID = executorId
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заказ передан в работу. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
