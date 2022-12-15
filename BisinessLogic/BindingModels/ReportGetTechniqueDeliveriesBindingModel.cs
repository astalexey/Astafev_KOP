using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.BindingModels
{
    public class ReportGetTechniqueDeliveriesBindingModel
    {
        public string FileName { get; set; }
        public int CustomerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
