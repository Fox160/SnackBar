using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSnackBar
{
    /// <summary>
    /// Логика взаимодействия для FormCustomerBookings.xaml
    /// </summary>
    public partial class FormCustomerBookings : Window
    {
        private bool _isReportViewerLoaded;

        public FormCustomerBookings()
        {
            InitializeComponent();
            reportViewer.Load += FormCustomerBookings_Load;
        }

        private void FormCustomerBookings_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                string exeFolder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                reportViewer.LocalReport.ReportPath = exeFolder + @"\ReportCustomerBookings.rdlc";

                reportViewer.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.SelectedDate.Value.Date.ToShortDateString() +
                                            " по " + dateTimePickerTo.SelectedDate.Value.Date.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameter);

                var response = APICustomer.PostRequest("api/Report/GetCustomerBookings", new BoundReportModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate.Value,
                    DateTo = dateTimePickerTo.SelectedDate.Value
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    var dataSource = APICustomer.GetElement<List<ModelCustomerBookingsView>>(response);
                    ReportDataSource source = new ReportDataSource("DataSetBookings", dataSource);
                    reportViewer.LocalReport.DataSources.Add(source);
                }
                else
                {
                    throw new Exception(APICustomer.GetError(response));
                }

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APICustomer.PostRequest("api/Report/SaveCustomerBookings", new BoundReportModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate.Value,
                        DateTo = dateTimePickerTo.SelectedDate.Value
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APICustomer.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
