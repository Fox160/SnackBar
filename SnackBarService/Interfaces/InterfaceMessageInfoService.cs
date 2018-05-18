using SnackBarService.DataFromUser;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.Interfaces
{
    public interface InterfaceMessageInfoService
    {
        List<ModelMessageInfoView> GetList();

        ModelMessageInfoView GetElement(int id);

        void AddElement(BoundMessageInfoModel model);
    }
}
