using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseImplements.Models
{
    public class SupplyOrder
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Supply Supply { get; set; }
        public virtual Order Order { get; set; }
    }
}
