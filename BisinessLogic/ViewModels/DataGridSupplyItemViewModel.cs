using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class DataGridSupplyItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Заказ")]
        public string OrderName { get; set; }

        [DisplayName("Стоимость")]
        public decimal? Price { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
