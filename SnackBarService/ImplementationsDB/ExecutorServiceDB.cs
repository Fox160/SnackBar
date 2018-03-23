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
    public class ExecutorServiceDB : InterfaceExecutorService
    {
        private AbstractDbContext context;

        public ExecutorServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelExecutorView> getList()
        {
            List<ModelExecutorView> result = context.Executors
                .Select(rec => new ModelExecutorView
                {
                    ID = rec.ID,
                    ExecutorFullName = rec.ExecutorFullName
                })
                .ToList();
            return result;
        }

        public ModelExecutorView getElement(int id)
        {
            Executor element = context.Executors.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelExecutorView
                {
                    ID = element.ID,
                    ExecutorFullName = element.ExecutorFullName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundExecutorModel model)
        {
            Executor element = context.Executors.FirstOrDefault(rec => rec.ExecutorFullName == model.ExecutorFullName);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Executors.Add(new Executor
            {
                ExecutorFullName = model.ExecutorFullName
            });
            context.SaveChanges();
        }

        public void updateElement(BoundExecutorModel model)
        {
            Executor element = context.Executors.FirstOrDefault(rec =>
                                        rec.ExecutorFullName == model.ExecutorFullName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Executors.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ExecutorFullName = model.ExecutorFullName;
            context.SaveChanges();
        }

        public void deleteElement(int id)
        {
            Executor element = context.Executors.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                context.Executors.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
