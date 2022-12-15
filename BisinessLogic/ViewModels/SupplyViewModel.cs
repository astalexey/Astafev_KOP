using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class SupplyViewModel
    {
        [DisplayName("Номер поставки")]
        public int Id { get; set; }
        [DisplayName("Поставка")]
        public string SupplyName { get; set; }

        [DisplayName("Цена")]
        public decimal TotalCost { get; set; }

        [DisplayName("Дата поставки")]
        public DateTime Date { get; set; }
        public Dictionary<int, (string, int)> SupplyOrders { get; set; }
    }
}
