using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace SnackBarView
{
    public partial class FormOutput : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceOutputService service;

        private int? id;

        private List<ModelProdElementView> productElements;

        public FormOutput(InterfaceOutputService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ModelProductView view = service.getElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ProductName;
                        textBoxPrice.Text = view.Price.ToString();
                        productElements = view.ProductElements;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                productElements = new List<ModelProdElementView>();
        }

        private void LoadData()
        {
            try
            {
                if (productElements != null)
                {
                    dataGridViewProduct.DataSource = null;
                    dataGridViewProduct.DataSource = productElements;
                    dataGridViewProduct.Columns[0].Visible = false;
                    dataGridViewProduct.Columns[1].Visible = false;
                    dataGridViewProduct.Columns[2].Visible = false;
                    dataGridViewProduct.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormOutputElement>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.ProductID = id.Value;
                    productElements.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormOutputElement>();
                form.Model = productElements[dataGridViewProduct.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productElements[dataGridViewProduct.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        productElements.RemoveAt(dataGridViewProduct.SelectedRows[0].Cells[0].RowIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (productElements == null || productElements.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<BoundProdElementModel> productComponentBM = new List<BoundProdElementModel>();
                for (int i = 0; i < productElements.Count; ++i)
                {
                    productComponentBM.Add(new BoundProdElementModel
                    {
                        ID = productElements[i].ID,
                        ProductID = productElements[i].ProductID,
                        ElementID = productElements[i].ElementID,
                        Count = productElements[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.updateElement(new BoundOutputModel
                    {
                        ID = id.Value,
                        ProductName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ProductElements = productComponentBM
                    });
                }
                else
                {
                    service.addElement(new BoundOutputModel
                    {
                        ProductName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ProductElements = productComponentBM
                    });
                }
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
