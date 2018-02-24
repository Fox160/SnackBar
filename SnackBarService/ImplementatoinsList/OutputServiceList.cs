using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;

namespace SnackBarService.ImplementationsList
{
    public class ProductServiceList : InterfaceOutputService
    {
        private DataListSingleton source;

        public ProductServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelProductView> getList()
        {
            List<ModelProductView> result = new List<ModelProductView>();
            for (int i = 0; i < source.Products.Count; ++i)
            {
                List<ModelProdElementView> productElements = new List<ModelProdElementView>();
                for (int j = 0; j < source.ProductElements.Count; ++j)
                {
                    if (source.ProductElements[j].ProductID == source.Products[i].ID)
                    {
                        string elementName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.ProductElements[j].ElementID == source.Elements[k].ID)
                            {
                                elementName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        productElements.Add(new ModelProdElementView
                        {
                            ID = source.ProductElements[j].ID,
                            ProductID = source.ProductElements[j].ProductID,
                            ElementID = source.ProductElements[j].ElementID,
                            ElementName = elementName,
                            Count = source.ProductElements[j].Count
                        });
                    }
                }
                result.Add(new ModelProductView
                {
                    ID = source.Products[i].ID,
                    ProductName = source.Products[i].ProductName,
                    Price = source.Products[i].Price,
                    ProductElements = productElements
                });
            }
            return result;
        }

        public ModelProductView getElement(int id)
        {
            for (int i = 0; i < source.Products.Count; ++i)
            {
                List<ModelProdElementView> productComponents = new List<ModelProdElementView>();
                for (int j = 0; j < source.ProductElements.Count; ++j)
                {
                    if (source.ProductElements[j].ProductID == source.Products[i].ID)
                    {
                        string elementName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.ProductElements[j].ElementID == source.Elements[k].ID)
                            {
                                elementName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        productComponents.Add(new ModelProdElementView
                        {
                            ID = source.ProductElements[j].ID,
                            ProductID = source.ProductElements[j].ProductID,
                            ElementID = source.ProductElements[j].ElementID,
                            ElementName = elementName,
                            Count = source.ProductElements[j].Count
                        });
                    }
                }
                if (source.Products[i].ID == id)
                {
                    return new ModelProductView
                    {
                        ID = source.Products[i].ID,
                        ProductName = source.Products[i].ProductName,
                        Price = source.Products[i].Price,
                        ProductElements = productComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundOutputModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Products.Count; ++i)
            {
                if (source.Products[i].ID > maxID)
                    maxID = source.Products[i].ID;
                if (source.Products[i].ProductName == model.ProductName)
                    throw new Exception("Уже есть изделие с таким названием");
            }
            source.Products.Add(new Output
            {
                ID = maxID + 1,
                ProductName = model.ProductName,
                Price = model.Price
            });
            int maxProductComponentID = 0;
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ID > maxProductComponentID)
                {
                    maxProductComponentID = source.ProductElements[i].ID;
                }
            }
            for (int i = 0; i < model.ProductElements.Count; ++i)
            {
                for (int j = 1; j < model.ProductElements.Count; ++j)
                {
                    if (model.ProductElements[i].ElementID == model.ProductElements[j].ElementID)
                    {
                        model.ProductElements[i].Count += model.ProductElements[j].Count;
                        model.ProductElements.RemoveAt(j--);
                    }
                }
            }
            for (int i = 0; i < model.ProductElements.Count; ++i)
            {
                source.ProductElements.Add(new OutputElement
                {
                    ID = ++maxProductComponentID,
                    ProductID = maxID + 1,
                    ElementID = model.ProductElements[i].ElementID,
                    Count = model.ProductElements[i].Count
                });
            }
        }

        public void updateElement(BoundOutputModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Products.Count; ++i)
            {
                if (source.Products[i].ID == model.ID)
                    index = i;
                if (source.Products[i].ProductName == model.ProductName && source.Products[i].ID != model.ID)
                    throw new Exception("Уже есть изделие с таким названием");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");

            source.Products[index].ProductName = model.ProductName;
            source.Products[index].Price = model.Price;
            int maxProductComponentID = 0;
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ID > maxProductComponentID)
                {
                    maxProductComponentID = source.ProductElements[i].ID;
                }
            }
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ProductID == model.ID)
                {
                    bool flag = true;
                    for (int j = 0; j < model.ProductElements.Count; ++j)
                    {
                        if (source.ProductElements[i].ID == model.ProductElements[j].ID)
                        {
                            source.ProductElements[i].Count = model.ProductElements[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        source.ProductElements.RemoveAt(i--);
                }
            }
            for (int i = 0; i < model.ProductElements.Count; ++i)
            {
                if (model.ProductElements[i].ID == 0)
                {
                    for (int j = 0; j < source.ProductElements.Count; ++j)
                    {
                        if (source.ProductElements[j].ProductID == model.ID &&
                            source.ProductElements[j].ElementID == model.ProductElements[i].ElementID)
                        {
                            source.ProductElements[j].Count += model.ProductElements[i].Count;
                            model.ProductElements[i].ID = source.ProductElements[j].ID;
                            break;
                        }
                    }
                    if (model.ProductElements[i].ID == 0)
                    {
                        source.ProductElements.Add(new OutputElement
                        {
                            ID = ++maxProductComponentID,
                            ProductID = model.ID,
                            ElementID = model.ProductElements[i].ElementID,
                            Count = model.ProductElements[i].Count
                        });
                    }
                }
            }
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.ProductElements.Count; ++i)
            {
                if (source.ProductElements[i].ProductID == id)
                    source.ProductElements.RemoveAt(i--);
            }
            for (int i = 0; i < source.Products.Count; ++i)
            {
                if (source.Products[i].ID == id)
                {
                    source.Products.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
