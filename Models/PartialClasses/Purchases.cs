using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    public partial class Purchases
    {
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Total")]
        public double? Amount { get; set; }
    }
}
