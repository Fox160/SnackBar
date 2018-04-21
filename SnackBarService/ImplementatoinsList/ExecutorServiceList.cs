using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class ExecutorServiceList : InterfaceExecutorService
    {
        private DataListSingleton source;

        public ExecutorServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelExecutorView> getList()
        {
            List<ModelExecutorView> result = source.Executors
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
            Executor element = source.Executors.FirstOrDefault(rec => rec.ID == id);
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
            Executor element = source.Executors.FirstOrDefault(rec => rec.ExecutorFullName == model.ExecutorFullName);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Executors.Count > 0 ? source.Executors.Max(rec => rec.ID) : 0;
            source.Executors.Add(new Executor
            {
                ID = maxId + 1,
                ExecutorFullName = model.ExecutorFullName
            });
        }

        public void updateElement(BoundExecutorModel model)
        {
            Executor element = source.Executors.FirstOrDefault(rec =>
                                        rec.ExecutorFullName == model.ExecutorFullName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Executors.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
                throw new Exception("Элемент не найден");

            element.ExecutorFullName = model.ExecutorFullName;
        }

        public void deleteElement(int id)
        {
            Executor element = source.Executors.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
                source.Executors.Remove(element);
            else
                throw new Exception("Элемент не найден");
        }
    }
}
