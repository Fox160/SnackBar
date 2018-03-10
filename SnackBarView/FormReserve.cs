using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace SnackBarView
{
    public partial class FormReserve : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly InterfaceReserveService service;

        private int? id;

        public FormReserve(InterfaceReserveService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormStock_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ModelReserveView view = service.getElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ReserveName;
                        dataGridViewReserve.DataSource = view.ReserveElements;
                        dataGridViewReserve.Columns[0].Visible = false;
                        dataGridViewReserve.Columns[1].Visible = false;
                        dataGridViewReserve.Columns[2].Visible = false;
                        dataGridViewReserve.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                if (id.HasValue)
                {
                    service.updateElement(new BoundReserveModel
                    {
                        ID = id.Value,
                        ReserveName = textBoxName.Text
                    });
                }
                else
                {
                    service.addElement(new BoundReserveModel
                    {
                        ReserveName = textBoxName.Text
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
