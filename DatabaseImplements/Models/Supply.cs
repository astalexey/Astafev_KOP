using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseImplements.Models
{
    public class Supply
    {
        public int Id { get; set; }
        [Required]
        public string SupplyName { get; set; }
        [Required]
        public decimal TotalCost { get; internal set; }
        public DateTime Date { get; set; }
        [ForeignKey("SupplyId")]
        public virtual List<SupplyOrder> SupplyOrders { get; set; }
        [ForeignKey("SupplyId")]
        public virtual List<SupplyGetTechnique> SupplyGetTechniques { get; set; }
    }
}
