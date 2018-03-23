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
    public class ElementServiceDB : InterfaceComponentService
    {
        private AbstractDbContext context;

        public ElementServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelElementView> getList()
        {
            List<ModelElementView> result = context.Elements
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
            Element element = context.Elements.FirstOrDefault(rec => rec.ID == id);
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
            Element element = context.Elements.FirstOrDefault(rec => rec.ElementName == model.ElementName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Elements.Add(new Element
            {
                ElementName = model.ElementName
            });
            context.SaveChanges();
        }

        public void updateElement(BoundElementModel model)
        {
            Element element = context.Elements.FirstOrDefault(rec =>
                                        rec.ElementName == model.ElementName && rec.ID != model.ID);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.Elements.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ElementName = model.ElementName;
            context.SaveChanges();
        }

        public void deleteElement(int id)
        {
            Element element = context.Elements.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                context.Elements.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
