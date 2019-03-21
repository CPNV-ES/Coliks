using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    public partial class Purchases
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        
        public virtual Customers Customer { get; set; }
    }
}
