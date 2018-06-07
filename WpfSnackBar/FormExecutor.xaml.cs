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
    /// Логика взаимодействия для FormExecutor.xaml
    /// </summary>
    public partial class FormExecutor : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceExecutorService service;

        private int? id;

        public FormExecutor(InterfaceExecutorService service)
        {
            InitializeComponent();
            Loaded += FormExecutor_Load;
            this.service = service;
        }

        private void FormExecutor_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ModelExecutorView view = service.getElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.ExecutorFullName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.updateElement(new BoundExecutorModel
                    {
                        ID = id.Value,
                        ExecutorFullName = textBoxFullName.Text
                    });
                }
                else
                {
                    service.addElement(new BoundExecutorModel
                    {
                        ExecutorFullName = textBoxFullName.Text
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
