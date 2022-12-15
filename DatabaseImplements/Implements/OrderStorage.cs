using BisinessLogic.BindingModels;
using BisinessLogic.Interfaces;
using BisinessLogic.ViewModels;
using DatabaseImplements.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseImplements.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.Orders
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    OrderName = rec.OrderName,
                    Price = rec.Price
                })
                .ToList();
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {
                return context.Orders
                .Where(rec => rec.OrderName.Contains(model.OrderName))
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    OrderName = rec.OrderName,
                    Price = rec.Price,
                })
                .ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {
                var tmp = model.CustomerId;
                var cosmetic = context.Orders
                .FirstOrDefault(rec => rec.OrderName == model.OrderName || rec.Id == model.Id);
                return cosmetic != null ?
                new OrderViewModel
                {
                    Id = cosmetic.Id,
                    OrderName = cosmetic.OrderName,
                    Price = cosmetic.Price
                } :
                null;
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new Database())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges();
            }
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new Database())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new Database())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.OrderName = model.OrderName;
            order.Price = model.Price;
            return order;
        }
    }
}
