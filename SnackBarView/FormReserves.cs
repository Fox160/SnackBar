using SnackBarService.Interfaces;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
                var response = APICustomer.GetRequest("api/Reserve/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ModelReserveView> list = APICustomer.GetElement<List<ModelReserveView>>(response);
                    if (list != null)
                    {
                        dataGridViewReserves.DataSource = list;
                        dataGridViewReserves.Columns[0].Visible = false;
                        dataGridViewReserves.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormReserve();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedRows.Count == 1)
            {
                var form = new FormReserve();
                form.Id = Convert.ToInt32(dataGridViewReserves.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewReserves.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewReserves.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        var response = APICustomer.PostRequest("api/Reserve/DelElement", new BoundCustomerModel { ID = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APICustomer.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
