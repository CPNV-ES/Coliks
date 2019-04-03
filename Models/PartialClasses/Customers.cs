using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    [ModelMetadataType(typeof(CustomersMetaData))]
    public partial class Customers
    {
    }

    public class CustomersMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Nom")]
        public string Lastname { get; set; }

        [Display(Name = "Prénom")]
        public string Firstname { get; set; }

        public string Address { get; set; }
        public int? CityId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public virtual Cities City { get; set; }
    }
}
