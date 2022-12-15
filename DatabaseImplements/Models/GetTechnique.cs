using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseImplements.Models
{
    public class GetTechnique // получение техники
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("GetTechniqueId")]
        public virtual List<SupplyGetTechnique> SupplyGetTechniques { get; set; }
    }
}
