using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System.Collections.Generic;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface InterfaceMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<ModelBookingView> getList();

        [CustomMethod("Метод создания заказа")]
        void createOrder(BoundBookingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void takeOrderInWork(BoundBookingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void finishOrder(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void payOrder(int id);

        [CustomMethod("Метод пополнения компонент на складе")]
        void putComponentOnReserve(BoundResElementModel model);
    }
}
