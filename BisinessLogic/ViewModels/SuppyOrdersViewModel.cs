using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class SuppyOrdersViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название заказа")]
        public string OrderName { get; set; }
        [DisplayName("Количество")]
        public int OrderCount { get; set; }
    }
}
