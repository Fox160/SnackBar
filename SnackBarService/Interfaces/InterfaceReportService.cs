using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;

namespace SnackBarService.Interfaces
{
    public interface InterfaceReportService
    {
        void SaveOutputPrice(BoundReportModel model);

        List<ModelReservesLoadView> GetReservesLoad();

        void SaveReservesLoad(BoundReportModel model);

        List<ModelCustomerBookingsView> GetCustomerBookings(BoundReportModel model);

        void SaveCustomerBookings(BoundReportModel model);
    }
}
