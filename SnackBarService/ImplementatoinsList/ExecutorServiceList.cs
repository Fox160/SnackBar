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
            List<ModelExecutorView> result = new List<ModelExecutorView>();
            for (int i = 0; i < source.Executors.Count; ++i)
            {
                result.Add(new ModelExecutorView
                {
                    ID = source.Executors[i].ID,
                    ExecutorFullName = source.Executors[i].ExecutorFullName
                });
            }
            return result;
        }

        public ModelExecutorView getElement(int id)
        {
            for (int i = 0; i < source.Executors.Count; ++i)
            {
                if (source.Executors[i].ID == id)
                {
                    return new ModelExecutorView
                    {
                        ID = source.Executors[i].ID,
                        ExecutorFullName = source.Executors[i].ExecutorFullName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundExecutorModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Executors.Count; ++i)
            {
                if (source.Executors[i].ID > maxID)
                    maxID = source.Executors[i].ID;
                if (source.Executors[i].ExecutorFullName == model.ExecutorFullName)
                    throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            source.Executors.Add(new Executor
            {
                ID = maxID + 1,
                ExecutorFullName = model.ExecutorFullName
            });
        }

        public void updateElement(BoundExecutorModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Executors.Count; ++i)
            {
                if (source.Executors[i].ID == model.ID)
                    index = i;
                if (source.Executors[i].ExecutorFullName == model.ExecutorFullName && source.Executors[i].ID != model.ID)
                    throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Executors[index].ExecutorFullName = model.ExecutorFullName;
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.Executors.Count; ++i)
            {
                if (source.Executors[i].ID == id)
                {
                    source.Executors.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
