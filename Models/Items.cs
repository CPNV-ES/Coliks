using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Items
    {
        public Items()
        {
            Renteditems = new HashSet<Renteditems>();
        }

        public int Id { get; set; }
        public string Itemnb { get; set; }
        public string Model { get; set; }
        public int? Size { get; set; }
        public int CategoryId { get; set; }
        public int? Cost { get; set; }
        public int? Returned { get; set; }
        public string Type { get; set; }
        public int? Stock { get; set; }
        public string Serialnumber { get; set; }
        public bool IsDeleted { get; set; }
        public int? BrandId { get; set; }

        public virtual Brands Brand { get; set; }
        public virtual Categories Category { get; set; }
        public virtual ICollection<Renteditems> Renteditems { get; set; }
    }
}
