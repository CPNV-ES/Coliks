using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Rentprices
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int DurationId { get; set; }
        public int GeartypeId { get; set; }
        public int? Price { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Durations Duration { get; set; }
        public virtual Geartypes Geartype { get; set; }
    }
}
