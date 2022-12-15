using BisinessLogic.BindingModels;
using BisinessLogic.Interfaces;
using BisinessLogic.ViewModels;
using DatabaseImplements.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseImplements.Implements
{
    public class GetTechniqueStorage : IGetTechniqueStorage
    {
        public List<GetTechniqueViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.GetTechniquies
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                .Include(rec => rec.Customer)
                .ToList()
                .Select(rec => new GetTechniqueViewModel
                {
                    Id = rec.Id,
                    ArrivalTime = rec.ArrivalTime,
                    SupplyGetTechnique = rec.SupplyGetTechniques
                            .ToDictionary(recS => recS.SupplyId,
                            recS => recS.Supply?.SupplyName)
                })
                .ToList();
            }
        }

        public List<GetTechniqueViewModel> GetFilteredList(GetTechniqueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {
                return context.GetTechniquies
                .Include(rec => rec.Customer)
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.CustomerId == model.CustomerId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.CustomerId == model.CustomerId && rec.ArrivalTime.Date >= model.DateFrom.Value.Date && rec.ArrivalTime.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new GetTechniqueViewModel
                {
                    Id = rec.Id,
                    ArrivalTime = rec.ArrivalTime,
                    SupplyGetTechnique = rec.SupplyGetTechniques
                            .ToDictionary(recS => recS.SupplyId,
                            recS => recS.Supply?.SupplyName)
                })
                .ToList();
            }
        }

        public GetTechniqueViewModel GetElement(GetTechniqueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                GetTechnique distribution = context.GetTechniquies
                .Include(rec => rec.Customer)
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                .FirstOrDefault(rec => rec.ArrivalTime == model.ArrivalTime || rec.Id == model.Id);
                return distribution != null ? new GetTechniqueViewModel
                {
                    Id = distribution.Id,
                    ArrivalTime = distribution.ArrivalTime,
                    SupplyGetTechnique = distribution.SupplyGetTechniques
                            .ToDictionary(recS => recS.SupplyId,
                            recS => recS.Supply?.SupplyName)
                } : null;
            }
        }

        public void Insert(GetTechniqueBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new GetTechnique(), context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(GetTechniqueBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        GetTechnique element = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(GetTechniqueBindingModel model)
        {
            using (var context = new Database())
            {
                GetTechnique element = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.GetTechniquies.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private GetTechnique CreateModel(GetTechniqueBindingModel model, GetTechnique getTechnique, Database context)
        {
            getTechnique.ArrivalTime = model.ArrivalTime;
            getTechnique.CustomerId = (int)model.CustomerId;

            if (getTechnique.Id == 0)
            {
                context.GetTechniquies.Add(getTechnique);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var sgt = context.SupplyGetTechniques
                     .Where(rec => rec.GetTechniqueId == model.Id.Value)
                     .ToList();

                context.SupplyGetTechniques.RemoveRange(sgt.ToList());

                context.SaveChanges();
            }

            foreach (var sgt in model.SupplyGetTechnique)
            {
                context.SupplyGetTechniques.Add(new SupplyGetTechnique
                {
                    GetTechniqueId = getTechnique.Id,
                    SupplyId = sgt.Key
                });
                context.SaveChanges();
            }
            return getTechnique;
        }
    }
}
