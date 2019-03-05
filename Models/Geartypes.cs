using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Geartypes
    {
        public Geartypes()
        {
            Rentprices = new HashSet<Rentprices>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Rentprices> Rentprices { get; set; }
    }
}
