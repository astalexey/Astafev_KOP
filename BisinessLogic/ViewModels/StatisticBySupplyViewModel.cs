using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class StatisticBySupplyViewModel
    {
        [DisplayName("Название поставки")]
        public string SupplyName { get; set; }

        [DisplayName("Количество компонент")]
        public int ComponentCount { get; set; }
    }
}
