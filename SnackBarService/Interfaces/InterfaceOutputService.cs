using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceOutputService
    {
        List<ModelOutputView> getList();

        ModelOutputView getElement(int id);

        void addElement(BoundOutputModel model);

        void updateElement(BoundOutputModel model);

        void deleteElement(int id);
    }
}
