using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnackBarView
{
    public partial class FormReserve : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormReserve()
        {
            InitializeComponent();
        }

        private void FormStock_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var stock = Task.Run(() => APICustomer.GetRequestData<ModelReserveView>("api/Reserve/Get/" + id.Value)).Result;
                    textBoxName.Text = stock.ReserveName;
                    dataGridViewReserve.DataSource = stock.ReserveElements;
                    dataGridViewReserve.Columns[0].Visible = false;
                    dataGridViewReserve.Columns[1].Visible = false;
                    dataGridViewReserve.Columns[2].Visible = false;
                    dataGridViewReserve.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Reserve/UpdElement", new BoundReserveModel
                {
                    ID = id.Value,
                    ReserveName = name
                }));
            }
            else
            {
                task = Task.Run(() => APICustomer.PostRequestData("api/Reserve/AddElement", new BoundReserveModel
                {
                    ReserveName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
