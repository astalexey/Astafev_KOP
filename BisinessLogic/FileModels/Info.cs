using BisinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLogic.FileModels
{
    public class Info
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportGetTechniqueViewModel> GetTechniques { get; set; }
        public List<ReportGetTechniqueDeliveriesViewModel> GetTechniqueDeliverysForPdf { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateFrom { get; set; }
    }
}
