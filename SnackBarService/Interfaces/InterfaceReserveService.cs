using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы со складами")]
    public interface InterfaceReserveService
    {
        [CustomMethod("Метод получения списка складов")]
        List<ModelReserveView> getList();

        [CustomMethod("Метод получения склада по id")]
        ModelReserveView getElement(int id);

        [CustomMethod("Метод добавления склада")]
        void addElement(BoundReserveModel model);

        [CustomMethod("Метод изменения данных по складу")]
        void updateElement(BoundReserveModel model);

        [CustomMethod("Метод удаления склада")]
        void deleteElement(int id);
    }
}
