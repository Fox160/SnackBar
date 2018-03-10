using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class ClientServiceList : InterfaceCustomerService
    {
        private DataListSingleton source;

        public ClientServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelCustomerView> getList()
        {
            List<ModelCustomerView> result = source.Customers
                .Select(rec => new ModelCustomerView
                {
                    ID = rec.ID,
                    CustomerFullName = rec.CustomerFullName
                })
                .ToList();
            return result;
        }

        public ModelCustomerView getElement(int id)
        {
            Customer element = source.Customers.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelCustomerView
                {
                    ID = element.ID,
                    CustomerFullName = element.CustomerFullName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundCustomerModel model)
        {
            Customer element = source.Customers.FirstOrDefault(rec => rec.CustomerFullName == model.CustomerFullName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            int maxId = source.Customers.Count > 0 ? source.Customers.Max(rec => rec.ID) : 0;
            source.Customers.Add(new Customer
            {
                ID = maxId + 1,
                CustomerFullName = model.CustomerFullName
            });
        }

        public void updateElement(BoundCustomerModel model)
        {
            Customer element = source.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFullName == model.CustomerFullName && rec.ID != model.ID);
            if (element != null)
                throw new Exception("Уже есть клиент с таким ФИО");

            element = source.Customers.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
                throw new Exception("Элемент не найден");

            element.CustomerFullName = model.CustomerFullName;
        }

        public void deleteElement(int id)
        {
            Customer element = source.Customers.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
                source.Customers.Remove(element);
            else
                throw new Exception("Элемент не найден");
        }
    }
}
