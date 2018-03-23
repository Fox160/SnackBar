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
    public class OutputServiceDB : InterfaceOutputService
    {
        private AbstractDbContext context;

        public OutputServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ModelOutputView> getList()
        {
            List<ModelOutputView> result = context.Outputs
                .Select(rec => new ModelOutputView
                {
                    ID = rec.ID,
                    OutputName = rec.OutputName,
                    Price = rec.Price,
                    OutputElements = context.OutputElements
                            .Where(recPC => recPC.OutputID == rec.ID)
                            .Select(recPC => new ModelProdElementView
                            {
                                ID = recPC.ID,
                                OutputID = recPC.OutputID,
                                ElementID = recPC.ElementID,
                                ElementName = recPC.Element.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ModelOutputView getElement(int id)
        {
            Output element = context.Outputs.FirstOrDefault(rec => rec.ID == id);
            if (element != null)
            {
                return new ModelOutputView
                {
                    ID = element.ID,
                    OutputName = element.OutputName,
                    Price = element.Price,
                    OutputElements = context.OutputElements
                            .Where(recPC => recPC.OutputID == element.ID)
                            .Select(recPC => new ModelProdElementView
                            {
                                ID = recPC.ID,
                                OutputID = recPC.OutputID,
                                ElementID = recPC.ElementID,
                                ElementName = recPC.Element.ElementName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void addElement(BoundOutputModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Output element = context.Outputs.FirstOrDefault(rec => rec.OutputName == model.OutputName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Output
                    {
                        OutputName = model.OutputName,
                        Price = model.Price
                    };
                    context.Outputs.Add(element);
                    context.SaveChanges();
                    var groupComponents = model.OutputElements
                                                .GroupBy(rec => rec.ElementID)
                                                .Select(rec => new
                                                {
                                                    ComponentId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupComponent in groupComponents)
                    {
                        context.OutputElements.Add(new OutputElement
                        {
                            OutputID = element.ID,
                            ElementID = groupComponent.ComponentId,
                            Count = groupComponent.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void updateElement(BoundOutputModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Output element = context.Outputs.FirstOrDefault(rec =>
                                        rec.OutputName == model.OutputName && rec.ID != model.ID);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Outputs.FirstOrDefault(rec => rec.ID == model.ID);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.OutputName = model.OutputName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    var compIds = model.OutputElements.Select(rec => rec.ElementID).Distinct();
                    var updateComponents = context.OutputElements
                                                    .Where(rec => rec.OutputID == model.ID &&
                                                        compIds.Contains(rec.ElementID));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.Count = model.OutputElements
                                                        .FirstOrDefault(rec => rec.ID == updateComponent.ID).Count;
                    }
                    context.SaveChanges();
                    context.OutputElements.RemoveRange(
                                        context.OutputElements.Where(rec => rec.OutputID == model.ID &&
                                                                            !compIds.Contains(rec.ElementID)));
                    context.SaveChanges();
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
                        OutputElement elementPC = context.OutputElements
                                                .FirstOrDefault(rec => rec.OutputID == model.ID &&
                                                                rec.ElementID == groupComponent.ComponentId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupComponent.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.OutputElements.Add(new OutputElement
                            {
                                OutputID = model.ID,
                                ElementID = groupComponent.ComponentId,
                                Count = groupComponent.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void deleteElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Output element = context.Outputs.FirstOrDefault(rec => rec.ID == id);
                    if (element != null)
                    {
                        context.OutputElements.RemoveRange(
                                            context.OutputElements.Where(rec => rec.OutputID == id));
                        context.Outputs.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
