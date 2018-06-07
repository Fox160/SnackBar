using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface InterfaceOutputService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<ModelOutputView> getList();

        [CustomMethod("Метод получения изделия по id")]
        ModelOutputView getElement(int id);

        [CustomMethod("Метод добавления изделия")]
        void addElement(BoundOutputModel model);

        [CustomMethod("Метод изменения данных по изделию")]
        void updateElement(BoundOutputModel model);

        [CustomMethod("Метод удаления изделия")]
        void deleteElement(int id);
    }
}
