using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Purchases
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public double? Amount { get; set; }

        public virtual Customers Customer { get; set; }
    }
}
