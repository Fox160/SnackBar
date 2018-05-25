using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы с компонентами")]
    public interface InterfaceComponentService
    {
        [CustomMethod("Метод получения списка компонент")]
        List<ModelElementView> getList();

        [CustomMethod("Метод получения компонента по id")]
        ModelElementView getElement(int id);

        [CustomMethod("Метод добавления компонента")]
        void addElement(BoundElementModel model);

        [CustomMethod("Метод изменения данных по компоненту")]
        void updateElement(BoundElementModel model);

        [CustomMethod("Метод удаления компонента")]
        void deleteElement(int id);
    }
}
