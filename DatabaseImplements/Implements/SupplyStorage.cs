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
    public class SupplyStorage : ISupplyStorage
    {
        public List<SupplyViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.Supplies
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .ToList()
                .Select(rec => new SupplyViewModel
                {
                    Id = rec.Id,
                    TotalCost = rec.TotalCost,
                    SupplyName = rec.SupplyName,
                    Date = rec.Date,
                    SupplyOrders = rec.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Count))
                })
                .ToList();
            }
        }
        public List<SupplyViewModel> GetFilteredList(SupplyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {

                return context.Supplies
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .Where(rec => model.DateFrom.HasValue && model.DateTo.HasValue && rec.Date.Date >= model.DateFrom.Value.Date && rec.Date.Date <= model.DateTo.Value.Date)
                .ToList()
                .Select(rec => new SupplyViewModel
                {
                    Id = rec.Id,
                    SupplyName = rec.SupplyName,
                    TotalCost = rec.TotalCost,
                    Date = rec.Date.Date,
                    SupplyOrders = rec.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Count))
                })
                .ToList();
            }
        }
        public SupplyViewModel GetElement(SupplyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                Supply supply = context.Supplies
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .FirstOrDefault(rec => rec.Date == model.Date || rec.Id == model.Id);
                return supply != null ? new SupplyViewModel
                {
                    Id = supply.Id,
                    SupplyName = supply.SupplyName,
                    TotalCost = supply.TotalCost,
                    Date = supply.Date,
                    SupplyOrders = supply.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Count))
                } : null;
            }
        }
        public void Insert(SupplyBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Supply(), context);
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
        public void Update(SupplyBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Supply element = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);

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

        public void Delete(SupplyBindingModel model)
        {
            using (var context = new Database())
            {
                Supply element = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Supplies.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Supply CreateModel(SupplyBindingModel model, Supply supply, Database context)
        {
            supply.SupplyName = model.SupplyName;
            supply.TotalCost = model.TotalCost;
            supply.Date = model.Date;

            if (supply.Id == 0)
            {
                context.Supplies.Add(supply);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var supplyOrders = context.SupplyOrders
                    .Where(rec => rec.SupplyId == model.Id.Value)
                    .ToList();

                context.SupplyOrders.RemoveRange(supplyOrders.ToList());

                context.SaveChanges();
            }

            foreach (var supplyOrder in model.SupplyOrders)
            {
                context.SupplyOrders.Add(new SupplyOrder
                {
                    SupplyId = supply.Id,
                    OrderId = supplyOrder.Key,
                    Count = supplyOrder.Value.Item2
                });
                context.SaveChanges();
            }
            return supply;
        }
    }
}
