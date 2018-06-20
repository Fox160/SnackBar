using System;
using SnackBarService.ViewModel;
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
    /// Логика взаимодействия для FormMails.xaml
    /// </summary>
    public partial class FormMails : Window
    {
        public FormMails()
        {
            InitializeComponent();
            Loaded += FormMails_Load;
        }

        private void FormMails_Load(object sender, EventArgs e)
        {
            try
            {
                MailClient.CheckMail();
                List<ModelMessageInfoView> list = Task.Run(() =>
                APICustomer.GetRequestData<List<ModelMessageInfoView>>("api/MessageInfo/GetList")).Result;
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[4].Width = DataGridLength.Auto;
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
    }
}
