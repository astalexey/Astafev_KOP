using BisinessLogic.BindingModels;
using BisinessLogic.Interfaces;
using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.BusinessLogics
{
    public class OrderLogic
    {
        private readonly IOrderStorage _orderStorage;
        public OrderLogic(IOrderStorage orderStorage)
        {
            _orderStorage = orderStorage;
        }
        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return _orderStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { _orderStorage.GetElement(model) };
            }
            return _orderStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(OrderBindingModel model)
        {
            var element = _orderStorage.GetElement(new OrderBindingModel
            {
                OrderName = model.OrderName,
                Price = model.Price
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Заказ уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _orderStorage.Update(model);
            }
            else
            {
                _orderStorage.Insert(model);
            }
        }

        public void Delete(OrderBindingModel model)
        {
            var element = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Заказ не найден");
            }
            _orderStorage.Delete(model);
        }
    }
}
