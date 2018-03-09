using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class ComponentServiceList : InterfaceComponentService
    {
        private DataListSingleton source;

        public ComponentServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelElementView> getList()
        {
            List<ModelElementView> result = source.Elements
                .Select(rec => new ModelElementView
                {
                    ID = rec.ID,
                    ElementName = rec.ElementName
                })
                .ToList();
            return result;
        }

        public ModelElementView getElement(int id)
        {
            Element element = source.Elements.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelElementView
                {
                    ID = element.ID,
                    ElementName = element.ElementName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundElementModel model)
        {
            Element element = source.Elements.FirstOrDefault(rec => rec.ElementName == model.ElementName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Elements.Count > 0 ? source.Elements.Max(rec => rec.ID) : 0;
            source.Elements.Add(new Element
            {
                ID = maxId + 1,
                ElementName = model.ElementName
            });
        }

        public void updateElement(BoundElementModel model)
        {
            Element element = source.Elements.FirstOrDefault(rec =>
                                       rec.ElementName == model.ElementName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Elements.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ElementName = model.ElementName;
        }

        public void deleteElement(int id)
        {
            Element element = source.Elements.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                source.Elements.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
