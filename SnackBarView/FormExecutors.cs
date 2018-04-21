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
    public partial class FormExecutors : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceExecutorService service;

        public FormExecutors(InterfaceExecutorService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormImplementers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ModelExecutorView> list = service.getList();
                if (list != null)
                {
                    dataGridViewExecutors.DataSource = list;
                    dataGridViewExecutors.Columns[0].Visible = false;
                    dataGridViewExecutors.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormExecutor>();
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewExecutors.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormExecutor>();
                form.ID = Convert.ToInt32(dataGridViewExecutors.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewExecutors.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewExecutors.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        service.deleteElement(id);
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
