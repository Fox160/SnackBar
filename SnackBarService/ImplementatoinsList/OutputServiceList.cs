using SnackBarModel;
using SnackBarService.DataFromUser;
using SnackBarService.Interfaces;
using SnackBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackBarService.ImplementationsList
{
    public class ProductServiceList : InterfaceOutputService
    {
        private DataListSingleton source;

        public ProductServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ModelOutputView> getList()
        {
            List<ModelOutputView> result = source.Outputs
                .Select(rec => new ModelOutputView
                {
                    ID = rec.ID,
                    OutputName = rec.OutputName,
                    Price = rec.Price,
                    OutputElements = source.OutputElements
                            .Where(recPC => recPC.OutputID == rec.ID)
                            .Select(recPC => new ModelProdElementView
                            {
                                ID = recPC.ID,
                                OutputID = recPC.OutputID,
                                ElementID = recPC.ElementID,
                                ElementName = source.Elements
                                    .FirstOrDefault(recC => recC.ID == recPC.ElementID)?.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ModelOutputView getElement(int id)
        {
            Output element = source.Outputs.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelOutputView
                {
                    ID = element.ID,
                    OutputName = element.OutputName,
                    Price = element.Price,
                    OutputElements = source.OutputElements
                            .Where(recPC => recPC.OutputID == element.ID)
                            .Select(recPC => new ModelProdElementView
                            {
                                ID = recPC.ID,
                                OutputID = recPC.OutputID,
                                ElementID = recPC.ElementID,
                                ElementName = source.Elements
                                        .FirstOrDefault(recC => recC.ID == recPC.ElementID)?.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundOutputModel model)
        {
            Output element = source.Outputs.FirstOrDefault(rec => rec.OutputName == model.OutputName);
            if (element != null)
                throw new Exception("Уже есть изделие с таким названием");
            int maxId = source.Outputs.Count > 0 ? source.Outputs.Max(rec => rec.ID) : 0;
            source.Outputs.Add(new Output
            {
                ID = maxId + 1,
                OutputName = model.OutputName,
                Price = model.Price
            });
            int maxPCId = source.OutputElements.Count > 0 ?
                                    source.OutputElements.Max(rec => rec.ID) : 0;
            var groupComponents = model.OutputElements
                                        .GroupBy(rec => rec.ElementID)
                                        .Select(rec => new
                                        {
                                            ComponentId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupComponent in groupComponents)
            {
                source.OutputElements.Add(new OutputElement
                {
                    ID = ++maxPCId,
                    OutputID = maxId + 1,
                    ElementID = groupComponent.ComponentId,
                    Count = groupComponent.Count
                });
            }
        }

        public void updateElement(BoundOutputModel model)
        {
            Output element = source.Outputs.FirstOrDefault(rec =>
                                        rec.OutputName == model.OutputName && rec.ID != model.ID);
            if (element != null)
                throw new Exception("Уже есть изделие с таким названием");
            element = source.Outputs.FirstOrDefault(rec => rec.ID == model.ID);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.OutputName = model.OutputName;
            element.Price = model.Price;

            int maxPCId = source.OutputElements.Count > 0 ? source.OutputElements.Max(rec => rec.ID) : 0;
            var compIds = model.OutputElements.Select(rec => rec.ElementID).Distinct();
            var updateComponents = source.OutputElements
                                            .Where(rec => rec.OutputID == model.ID &&
                                           compIds.Contains(rec.ElementID));
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Count = model.OutputElements
                                                .FirstOrDefault(rec => rec.ID == updateComponent.ID).Count;
            }
            source.OutputElements.RemoveAll(rec => rec.OutputID == model.ID &&
                                       !compIds.Contains(rec.ElementID));
            var groupComponents = model.OutputElements
                                        .Where(rec => rec.ID == 0)
                                        .GroupBy(rec => rec.ElementID)
                                        .Select(rec => new
                                        {
                                            ComponentId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupComponent in groupComponents)
            {
                OutputElement elementPC = source.OutputElements
                                        .FirstOrDefault(rec => rec.OutputID == model.ID &&
                                                        rec.ElementID == groupComponent.ComponentId);
                if (elementPC != null)
                {
                    elementPC.Count += groupComponent.Count;
                }
                else
                {
                    source.OutputElements.Add(new OutputElement
                    {
                        ID = ++maxPCId,
                        OutputID = model.ID,
                        ElementID = groupComponent.ComponentId,
                        Count = groupComponent.Count
                    });
                }
            }
        }

        public void deleteElement(int id)
        {
            Output element = source.Outputs.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                source.OutputElements.RemoveAll(rec => rec.OutputID == id);
                source.Outputs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
