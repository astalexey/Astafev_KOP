using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class SupplyGetTechiquesViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название поставки")]
        public string SupplyName { get; set; }
    }
}
