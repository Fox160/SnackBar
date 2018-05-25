using SnackBarService.Attributies;
using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.Interfaces
{
    [CustomInterface("Интерфейс для работы с письмами")]
    public interface InterfaceMessageInfoService
    {
        [CustomMethod("Метод получения списка писем")]
        List<ModelMessageInfoView> GetList();

        [CustomMethod("Метод получения письма по id")]
        ModelMessageInfoView GetElement(int id);

        [CustomMethod("Метод добавления письма")]
        void AddElement(BoundMessageInfoModel model);
    }
}
