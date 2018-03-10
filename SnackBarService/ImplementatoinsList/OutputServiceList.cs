using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class OutputserviceList : InterfaceOutputService
    {
        private DataListSingleton source;

        public OutputserviceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelOutputView> getList()
        {
            List<ModelOutputView> result = new List<ModelOutputView>();
            for (int i = 0; i < source.Outputs.Count; ++i)
            {
                List<ModelProdElementView> OutputElements = new List<ModelProdElementView>();
                for (int j = 0; j < source.OutputElements.Count; ++j)
                {
                    if (source.OutputElements[j].OutputID == source.Outputs[i].ID)
                    {
                        string elementName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.OutputElements[j].ElementID == source.Elements[k].ID)
                            {
                                elementName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        OutputElements.Add(new ModelProdElementView
                        {
                            ID = source.OutputElements[j].ID,
                            OutputID = source.OutputElements[j].OutputID,
                            ElementID = source.OutputElements[j].ElementID,
                            ElementName = elementName,
                            Count = source.OutputElements[j].Count
                        });
                    }
                }
                result.Add(new ModelOutputView
                {
                    ID = source.Outputs[i].ID,
                    OutputName = source.Outputs[i].OutputName,
                    Price = source.Outputs[i].Price,
                    OutputElements = OutputElements
                });
            }
            return result;
        }

        public ModelOutputView getElement(int id)
        {
            for (int i = 0; i < source.Outputs.Count; ++i)
            {
                List<ModelProdElementView> productComponents = new List<ModelProdElementView>();
                for (int j = 0; j < source.OutputElements.Count; ++j)
                {
                    if (source.OutputElements[j].OutputID == source.Outputs[i].ID)
                    {
                        string elementName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.OutputElements[j].ElementID == source.Elements[k].ID)
                            {
                                elementName = source.Elements[k].ElementName;
                                break;
                            }
                        }
                        productComponents.Add(new ModelProdElementView
                        {
                            ID = source.OutputElements[j].ID,
                            OutputID = source.OutputElements[j].OutputID,
                            ElementID = source.OutputElements[j].ElementID,
                            ElementName = elementName,
                            Count = source.OutputElements[j].Count
                        });
                    }
                }
                if (source.Outputs[i].ID == id)
                {
                    return new ModelOutputView
                    {
                        ID = source.Outputs[i].ID,
                        OutputName = source.Outputs[i].OutputName,
                        Price = source.Outputs[i].Price,
                        OutputElements = productComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundOutputModel model)
        {
            int maxID = 0;
            for (int i = 0; i < source.Outputs.Count; ++i)
            {
                if (source.Outputs[i].ID > maxID)
                    maxID = source.Outputs[i].ID;
                if (source.Outputs[i].OutputName == model.OutputName)
                    throw new Exception("Уже есть изделие с таким названием");
            }
            source.Outputs.Add(new Output
            {
                ID = maxID + 1,
                OutputName = model.OutputName,
                Price = model.Price
            });
            int maxProductComponentID = 0;
            for (int i = 0; i < source.OutputElements.Count; ++i)
            {
                if (source.OutputElements[i].ID > maxProductComponentID)
                {
                    maxProductComponentID = source.OutputElements[i].ID;
                }
            }
            for (int i = 0; i < model.OutputElements.Count; ++i)
            {
                for (int j = 1; j < model.OutputElements.Count; ++j)
                {
                    if (model.OutputElements[i].ElementID == model.OutputElements[j].ElementID)
                    {
                        model.OutputElements[i].Count += model.OutputElements[j].Count;
                        model.OutputElements.RemoveAt(j--);
                    }
                }
            }
            for (int i = 0; i < model.OutputElements.Count; ++i)
            {
                source.OutputElements.Add(new OutputElement
                {
                    ID = ++maxProductComponentID,
                    OutputID = maxID + 1,
                    ElementID = model.OutputElements[i].ElementID,
                    Count = model.OutputElements[i].Count
                });
            }
        }

        public void updateElement(BoundOutputModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Outputs.Count; ++i)
            {
                if (source.Outputs[i].ID == model.ID)
                    index = i;
                if (source.Outputs[i].OutputName == model.OutputName && source.Outputs[i].ID != model.ID)
                    throw new Exception("Уже есть изделие с таким названием");
            }
            if (index == -1)
                throw new Exception("Элемент не найден");

            source.Outputs[index].OutputName = model.OutputName;
            source.Outputs[index].Price = model.Price;
            int maxProductComponentID = 0;
            for (int i = 0; i < source.OutputElements.Count; ++i)
            {
                if (source.OutputElements[i].ID > maxProductComponentID)
                {
                    maxProductComponentID = source.OutputElements[i].ID;
                }
            }
            for (int i = 0; i < source.OutputElements.Count; ++i)
            {
                if (source.OutputElements[i].OutputID == model.ID)
                {
                    bool flag = true;
                    for (int j = 0; j < model.OutputElements.Count; ++j)
                    {
                        if (source.OutputElements[i].ID == model.OutputElements[j].ID)
                        {
                            source.OutputElements[i].Count = model.OutputElements[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        source.OutputElements.RemoveAt(i--);
                }
            }
            for (int i = 0; i < model.OutputElements.Count; ++i)
            {
                if (model.OutputElements[i].ID == 0)
                {
                    for (int j = 0; j < source.OutputElements.Count; ++j)
                    {
                        if (source.OutputElements[j].OutputID == model.ID &&
                            source.OutputElements[j].ElementID == model.OutputElements[i].ElementID)
                        {
                            source.OutputElements[j].Count += model.OutputElements[i].Count;
                            model.OutputElements[i].ID = source.OutputElements[j].ID;
                            break;
                        }
                    }
                    if (model.OutputElements[i].ID == 0)
                    {
                        source.OutputElements.Add(new OutputElement
                        {
                            ID = ++maxProductComponentID,
                            OutputID = model.ID,
                            ElementID = model.OutputElements[i].ElementID,
                            Count = model.OutputElements[i].Count
                        });
                    }
                }
            }
        }

        public void deleteElement(int id)
        {
            for (int i = 0; i < source.OutputElements.Count; ++i)
            {
                if (source.OutputElements[i].OutputID == id)
                    source.OutputElements.RemoveAt(i--);
            }
            for (int i = 0; i < source.Outputs.Count; ++i)
            {
                if (source.Outputs[i].ID == id)
                {
                    source.Outputs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
