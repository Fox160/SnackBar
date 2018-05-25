using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы с клиентами")]
    public interface InterfaceCustomerService
    {
        [CustomMethod("Метод получения списка клиентов")]
        List<ModelCustomerView> getList();

        [CustomMethod("Метод получения клиента по id")]
        ModelCustomerView getElement(int id);

        [CustomMethod("Метод добавления клиента")]
        void addElement(BoundCustomerModel model);

        [CustomMethod("Метод изменения данных по клиенту")]
        void updateElement(BoundCustomerModel model);

        [CustomMethod("Метод удаления клиента")]
        void deleteElement(int id);
    }
}
