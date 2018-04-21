using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SnackBarView
{
    public partial class FormPutOnReserve : Form
    {
        public FormPutOnReserve()
        {
            InitializeComponent();
        }

        private void FormPutOnStock_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APICustomer.GetRequest("api/Element/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<ModelElementView> list = APICustomer.GetElement<List<ModelElementView>>(responseC);
                    if (list != null)
                    {
                        comboBoxComponent.DisplayMember = "ElementName";
                        comboBoxComponent.ValueMember = "Id";
                        comboBoxComponent.DataSource = list;
                        comboBoxComponent.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseC));
                }
                var responseS = APICustomer.GetRequest("api/Reserve/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<ModelReserveView> list = APICustomer.GetElement<List<ModelReserveView>>(responseS);
                    if (list != null)
                    {
                        comboBoxStock.DisplayMember = "ReserveName";
                        comboBoxStock.ValueMember = "ID";
                        comboBoxStock.DataSource = list;
                        comboBoxStock.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APICustomer.GetError(responseC));
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
                var response = APICustomer.PostRequest("api/Main/PutElementOnReserve", new BoundResElementModel
                {
                    ElementID = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    ReserveID = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
