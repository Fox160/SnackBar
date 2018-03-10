using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class CustomerserviceList : InterfaceCustomerService
    {
        private DataListSingleton source;

        public CustomerserviceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelCustomerView> getList()
        {
            List<ModelCustomerView> result = new List<ModelCustomerView>();
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                result.Add(new ModelCustomerView
                {
                    ID = source.Customers[i].ID,
                    CustomerFullName = source.Customers[i].CustomerFullName
                });
            }
            return result;
        }

        public ModelCustomerView getElement(int id)
        {
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].ID == id)
                {
                    return new ModelCustomerView
                    {
                        ID = source.Customers[i].ID,
                        CustomerFullName = source.Customers[i].CustomerFullName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundCustomerModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].ID > maxID)
                    maxID = source.Customers[i].ID;
                if (source.Customers[i].CustomerFullName == model.CustomerFullName)
                    throw new Exception("Уже есть клиент с таким ФИО");
            }
            source.Customers.Add(new Customer
            {
                ID = maxID + 1,
                CustomerFullName = model.CustomerFullName
            });
        }

        public void updateElement(BoundCustomerModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].ID == model.ID)
                    index = i;
                if (source.Customers[i].CustomerFullName == model.CustomerFullName && source.Customers[i].ID != model.ID)
                    throw new Exception("Уже есть клиент с таким ФИО");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Customers[index].CustomerFullName = model.CustomerFullName;
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].ID == id)
                {
                    source.Customers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
