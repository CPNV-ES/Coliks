using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Contracts = new HashSet<Contracts>();
            Purchases = new HashSet<Purchases>();
        }

        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public virtual Cities City { get; set; }
        public virtual ICollection<Contracts> Contracts { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
    }
}
