using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }
        [DisplayName("Prenom")]
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [DisplayName("Adresse")]
        [StringLength(50)]
        public string Address { get; set; }
        public int? CityId { get; set; }
        [DisplayName("Téléphone")]
        [Phone]
        public string Phone { get; set; }
        [DisplayName("Email")]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [DisplayName("Mobile")]
        [Phone]
        public string Mobile { get; set; }

        [DisplayName("Ville")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Contracts> Contracts { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
    }
}
