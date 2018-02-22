using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;

namespace SnackBarService.ImplementationsList
{
    public class ComponentServiceList : InterfaceComponentService
    {
        private DataListSingleton source;

        public ComponentServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelComponentView> getList()
        {
            List<ModelComponentView> result = new List<ModelComponentView>();
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                result.Add(new ModelComponentView
                {
                    ID = source.Elements[i].ID,
                    ElementName = source.Elements[i].ElementName
                });
            }
            return result;
        }

        public ModelComponentView getElement(int id)
        {
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].ID == id)
                {
                    return new ModelComponentView
                    {
                        ID = source.Elements[i].ID,
                        ElementName = source.Elements[i].ElementName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundComponentModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].ID > maxID)
                    maxID = source.Elements[i].ID;
                if (source.Elements[i].ElementName == model.ElementName)
                    throw new Exception("Уже есть компонент с таким названием");
            }
            source.Elements.Add(new Element
            {
                ID = maxID + 1,
                ElementName = model.ElementName
            });
        }

        public void updateElement(BoundComponentModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].ID == model.ID)
                    index = i;
                if (source.Elements[i].ElementName == model.ElementName && source.Elements[i].ID != model.ID)
                    throw new Exception("Уже есть компонент с таким названием");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");
            source.Elements[index].ElementName = model.ElementName;
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].ID == id)
                {
                    source.Elements.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
