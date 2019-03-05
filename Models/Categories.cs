using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Items = new HashSet<Items>();
            Renteditems = new HashSet<Renteditems>();
            Rentprices = new HashSet<Rentprices>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Items> Items { get; set; }
        public virtual ICollection<Renteditems> Renteditems { get; set; }
        public virtual ICollection<Rentprices> Rentprices { get; set; }
    }
}
