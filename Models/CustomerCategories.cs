using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class CustomerCategories
    {
        public CustomerCategories()
        {
            Customers = new HashSet<Customers>();
        }

        public int Id { get; set; }
        public int? Totalamount { get; set; }
        public string Categoryname { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }
}
