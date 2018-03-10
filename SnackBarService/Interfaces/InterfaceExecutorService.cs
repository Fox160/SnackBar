using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    public interface InterfaceExecutorService
    {
        List<ModelExecutorView> getList();

        ModelExecutorView getElement(int id);

        void addElement(BoundExecutorModel model);

        void updateElement(BoundExecutorModel model);

        void deleteElement(int id);
    }
}
