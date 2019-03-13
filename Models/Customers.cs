using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [DisplayName("Nom")]
        public string Lastname { get; set; }
        [DisplayName("Prenom")]
        public string Firstname { get; set; }
        [DisplayName("Adresse")]
        public string Address { get; set; }
        public int? CityId { get; set; }
        [DisplayName("Téléphone")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Ville")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Contracts> Contracts { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
    }
}
