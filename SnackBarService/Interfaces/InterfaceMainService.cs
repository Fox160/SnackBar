using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceMainService
    {
        List<ModelBookingView> getList();

        void createOrder(BoundBookingModel model);

        void takeOrderInWork(BoundBookingModel model);

        void finishOrder(int id);

        void payOrder(int id);

        void putComponentOnReserve(BoundResElementModel model);
    }
}
