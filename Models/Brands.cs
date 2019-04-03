using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Brands
    {
        public Brands()
        {
            Items = new HashSet<Items>();
        }

        public int Id { get; set; }
        public string Brandname { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
