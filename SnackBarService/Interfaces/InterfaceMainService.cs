using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceMainService
    {
        List<OrderViewModel> getList();

        void createOrder(BoundOrderModel model);

        void takeOrderInWork(BoundOrderModel model);

        void finishOrder(int id);

        void payOrder(int id);

        void putComponentOnReserve(BoundResComponentModel model);
    }
}
