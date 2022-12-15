using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int CustomerId { get; set; }
        [DisplayName("Заказ")]
        public string OrderName { get; set; }
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> OrderSupplies { get; set; }
    }
}
