using System;
using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackBarService.ImplementationsDB
{
    public class CustomerServiceDB : InterfaceCustomerService
    {
        private AbstractDbContext context;

        public CustomerServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelCustomerView> getList()
        {
            List<ModelCustomerView> result = context.Customers
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
            Customer element = context.Customers.FirstOrDefault(rec => rec.ID == id);
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
            Customer element = context.Customers.FirstOrDefault(rec => rec.CustomerFullName == model.CustomerFullName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Customers.Add(new Customer
            {
                CustomerFullName = model.CustomerFullName
            });
            context.SaveChanges();
        }

        public void updateElement(BoundCustomerModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFullName == model.CustomerFullName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Customers.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFullName = model.CustomerFullName;
            context.SaveChanges();
        }

        public void deleteElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                context.Customers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
