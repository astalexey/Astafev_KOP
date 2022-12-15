using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BisinessLogic.ViewModels
{
    public class GetTechniqueViewModel
    {
        [DisplayName("ID прибытия")]
        public int Id { get; set; }

        [DisplayName("Время получения")]
        public DateTime? ArrivalTime { get; set; }
        public Dictionary<int, string> SupplyGetTechnique { get; set; }
    }
}
