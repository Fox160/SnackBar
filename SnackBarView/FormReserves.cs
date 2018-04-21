using SnackBarService.Interfaces;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SnackBarView
{
    public partial class FormReserves : Form
    {
        public FormReserves()
        {
            InitializeComponent();
        }

        private void FormStocks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ModelReserveView> list = Task.Run(() => APICustomer.GetRequestData<List<ModelReserveView>>("api/Reserve/GetList")).Result;
                if (list != null)
                {
                    dataGridViewReserves.DataSource = list;
                    dataGridViewReserves.Columns[0].Visible = false;
                    dataGridViewReserves.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormReserve();
            form.ShowDialog();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedRows.Count == 1)
            {
                var form = new FormReserve
                {
                    Id = Convert.ToInt32(dataGridViewReserves.SelectedRows[0].Cells[0].Value)
                };
                form.ShowDialog();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewReserves.SelectedRows[0].Cells[0].Value);

                    Task task = Task.Run(() => APICustomer.PostRequestData("api/Reserve/DelElement", new BoundCustomerModel { ID = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
