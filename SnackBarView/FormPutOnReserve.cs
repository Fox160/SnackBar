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
    public partial class FormPutOnReserve : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly InterfaceReserveService serviceReserve;

        private readonly InterfaceComponentService serviceComponent;

        private readonly InterfaceMainService serviceMain;

        public FormPutOnReserve(InterfaceReserveService serviceS, InterfaceComponentService serviceC, InterfaceMainService serviceM)
        {
            InitializeComponent();
            this.serviceReserve = serviceS;
            this.serviceComponent = serviceC;
            this.serviceMain = serviceM;
        }

        private void FormPutOnStock_Load(object sender, EventArgs e)
        {
            try
            {
                List<ModelElementView> listElement = serviceComponent.getList();
                if (listElement != null)
                {
                    comboBoxComponent.DisplayMember = "ElementName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = listElement;
                    comboBoxComponent.SelectedItem = null;
                }
                List<ModelReserveView> listReserve = serviceReserve.getList();
                if (listReserve != null)
                {
                    comboBoxStock.DisplayMember = "ReserveName";
                    comboBoxStock.ValueMember = "Id";
                    comboBoxStock.DataSource = listReserve;
                    comboBoxStock.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStock.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceMain.putComponentOnReserve(new BoundResElementModel
                {
                    ElementID = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    ReserveID = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
