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
            if (dateTimePickerFrom.SelectedDate == null || dateTimePickerTo.SelectedDate == null)
            {
                MessageBox.Show("Дата не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

                DateTime dateFrom = dateTimePickerFrom.SelectedDate.Value;
                DateTime dateTo = dateTimePickerTo.SelectedDate.Value;

                var dataSource = Task.Run(() => APICustomer.PostRequestData<BoundReportModel, List<ModelCustomerBookingsView>>("api/Report/GetCustomerBookings",
                new BoundReportModel
                {
                    DateFrom = dateFrom,
                    DateTo = dateTo
                })).Result;
                ReportDataSource source = new ReportDataSource("DataSetBookings", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                string fileName = sfd.FileName;
                DateTime dateFrom = dateTimePickerFrom.SelectedDate.Value;
                DateTime dateTo = dateTimePickerTo.SelectedDate.Value;

                Task task = Task.Run(() => APICustomer.PostRequestData("api/Report/SaveCustomerBookings", new BoundReportModel
                {
                    FileName = fileName,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Список заказов сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
