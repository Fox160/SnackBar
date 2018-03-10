using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceCustomerService
    {
        List<ModelCustomerView> getList();

        ModelCustomerView getElement(int id);

        void addElement(BoundCustomerModel model);

        void updateElement(BoundCustomerModel model);

        void deleteElement(int id);
    }
}
