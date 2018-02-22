using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceComponentService
    {
        List<ModelComponentView> getList();

        ModelComponentView getElement(int id);

        void addElement(BoundComponentModel model);

        void updateElement(BoundComponentModel model);

        void deleteElement(int id);
    }
}
