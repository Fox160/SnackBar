using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using SnackBarService.DataFromUser;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SnackBarView
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<ModelBookingView> list = Task.Run(() => APICustomer.GetRequestData<List<ModelBookingView>>("api/Main/GetList")).Result;
                if (list != null)
                {
                    dataGridViewMain.DataSource = list;
                    dataGridViewMain.Columns[0].Visible = false;
                    dataGridViewMain.Columns[1].Visible = false;
                    dataGridViewMain.Columns[3].Visible = false;
                    dataGridViewMain.Columns[5].Visible = false;
                    dataGridViewMain.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomers();
            form.ShowDialog();
        }

        private void компонентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormElements();
            form.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormOutputs();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormReserves();
            form.ShowDialog();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormExecutors();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPutOnReserve();
            form.ShowDialog();
        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = new FormCreateBooking();
            form.ShowDialog();
        }

        private void buttonTakeOrderInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 1)
            {
                var form = new FormTakeBookingInWork
                {
                    Id = Convert.ToInt32(dataGridViewMain.SelectedRows[0].Cells[0].Value)
                };
                form.ShowDialog();
            }
        }

        private void buttonOrderReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridViewMain.SelectedRows[0].Cells[0].Value);

                Task task = Task.Run(() =>
                    {
                        APICustomer.PostRequestData("api/Main/FinishBooking", new BoundBookingModel{ ID = id });
                    }
                );

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith(
                    (prevTask) =>
                        {
                            var ex = (Exception)prevTask.Exception;
                            while (ex.InnerException != null)
                            {
                                ex = ex.InnerException;
                            }
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }, 
                    TaskContinuationOptions.OnlyOnFaulted
                );
            }
        }

        private void buttonPayOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridViewMain.SelectedRows[0].Cells[0].Value);


                Task task = Task.Run(() =>
                    {
                        APICustomer.PostRequestData("api/Main/PayBooking", new BoundBookingModel { ID = id });
                    }
                );

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith(
                    (prevTask) =>
                        {
                            var ex = (Exception)prevTask.Exception;
                            while (ex.InnerException != null)
                            {
                                ex = ex.InnerException;
                            }
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }, 
                    TaskContinuationOptions.OnlyOnFaulted
                );
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсИзделийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APICustomer.PostRequestData("api/Report/SaveOutputPrice", new BoundReportModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

        private void загруженностьСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormReservesLoad();
            form.ShowDialog();
        }

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomerBookings();
            form.ShowDialog();
        }

        private void письмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormMails();
            form.ShowDialog();
        }
    }
}
