using Microsoft.Reporting.WinForms;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
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
        public partial class FormCustomerBookings : Form
        {
            [Dependency]
            public new IUnityContainer Container { get; set; }

            private readonly InterfaceReportService service;

            public FormCustomerBookings(InterfaceReportService service)
            {
                InitializeComponent();
                this.service = service;
            }

        private void FormCustomerBookings_Load(object sender, EventArgs e)
        {
            this.reportViewer.RefreshReport();
        }

        private void buttonMake_Click(object sender, EventArgs e)
            {
                if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
                {
                    MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                                "c " + dateTimePickerFrom.Value.ToShortDateString() +
                                                " по " + dateTimePickerTo.Value.ToShortDateString());
                    reportViewer.LocalReport.SetParameters(parameter);

                    var dataSource = service.GetCustomerBookings(new BoundReportModel
                    {
                        DateFrom = dateTimePickerFrom.Value,
                        DateTo = dateTimePickerTo.Value
                    });
                    ReportDataSource source = new ReportDataSource("DataSetBookings", dataSource);
                    reportViewer.LocalReport.DataSources.Add(source);

                    reportViewer.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void buttonToPdf_Click(object sender, EventArgs e)
            {
                if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
                {
                    MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "pdf|*.pdf"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        service.SaveCustomerBookings(new BoundReportModel
                        {
                            FileName = sfd.FileName,
                            DateFrom = dateTimePickerFrom.Value,
                            DateTo = dateTimePickerTo.Value
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }