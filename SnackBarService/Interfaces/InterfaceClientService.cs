using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceClientService
    {
        List<ModelClientView> getList();

        ModelClientView getElement(int id);

        void addElement(BoundClientModel model);

        void updateElement(BoundClientModel model);

        void deleteElement(int id);
    }
}
