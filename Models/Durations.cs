using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Durations
    {
        public Durations()
        {
            Renteditems = new HashSet<Renteditems>();
            Rentprices = new HashSet<Rentprices>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }

        public virtual ICollection<Renteditems> Renteditems { get; set; }
        public virtual ICollection<Rentprices> Rentprices { get; set; }
    }
}
