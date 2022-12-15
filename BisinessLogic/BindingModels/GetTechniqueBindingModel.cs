using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.BindingModels
{
    public class GetTechniqueBindingModel
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int SupplyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Dictionary<int, string> SupplyGetTechnique { get; set; }
    }
}
