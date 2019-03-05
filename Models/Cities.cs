using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Customers = new HashSet<Customers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }
}
