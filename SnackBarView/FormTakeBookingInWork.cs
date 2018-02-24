using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace SnackBarView
{
    public partial class FormTakeBookingInWork : Form
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
            this.serviceExecutor = serviceI;
            this.serviceMain = serviceM;
        }

        private void FormTakeOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                List<ModelExecutorView> listExecutor = serviceExecutor.getList();
                if (listExecutor != null)
                {
                    comboBoxExecutor.DisplayMember = "ExecutorFullName";
                    comboBoxExecutor.ValueMember = "Id";
                    comboBoxExecutor.DataSource = listExecutor;
                    comboBoxExecutor.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxExecutor.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceMain.takeOrderInWork(new BoundBookingModel
                {
                    ID = id.Value,
                    ExecutorID = Convert.ToInt32(comboBoxExecutor.SelectedValue)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
