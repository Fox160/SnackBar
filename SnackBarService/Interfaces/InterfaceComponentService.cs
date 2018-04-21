using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceComponentService
    {
        List<ModelElementView> getList();

        ModelElementView getElement(int id);

        void addElement(BoundElementModel model);

        void updateElement(BoundElementModel model);

        void deleteElement(int id);
    }
}
