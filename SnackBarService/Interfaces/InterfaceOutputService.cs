using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceOutputService
    {
        List<ModelProductView> getList();

        ModelProductView getElement(int id);

        void addElement(BoundOutputModel model);

        void updateElement(BoundOutputModel model);

        void deleteElement(int id);
    }
}
