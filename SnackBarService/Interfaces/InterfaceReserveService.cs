using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceReserveService
    {
        List<ModelReserveView> getList();

        ModelReserveView getElement(int id);

        void addElement(BoundReserveModel model);

        void updateElement(BoundReserveModel model);

        void deleteElement(int id);
    }
}
