using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceProductService
    {
        List<ModelProductView> getList();

        ModelProductView getElement(int id);

        void addElement(BoundProductModel model);

        void updateElement(BoundProductModel model);

        void deleteElement(int id);
    }
}
