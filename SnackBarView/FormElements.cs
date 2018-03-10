using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace SnackBarView
{
    public partial class FormElements : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceComponentService service;

        public FormElements(InterfaceComponentService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormComponents_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ModelElementView> list = service.getList();
                if (list != null)
                {
                    dataGridViewElements.DataSource = list;
                    dataGridViewElements.Columns[0].Visible = false;
                    dataGridViewElements.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormElement>();
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewElements.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormElement>();
                form.ID = Convert.ToInt32(dataGridViewElements.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewElements.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridViewElements.SelectedRows[0].Cells[0].Value);
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
