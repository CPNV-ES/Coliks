using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coliks.Models
{
    [ModelMetadataType(typeof(CustomersMetadata))]
    public partial class Customers
    {
        [NotMapped]
        public int? TotalPurchases { get; set; }
    }

    public class CustomersMetadata
    {
        [DisplayName("Nom")]
        [Required(ErrorMessage = "Le nom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        [Remote(action: "VerifyName", controller: "Customers", AdditionalFields = "Firstname, Id", ErrorMessage = "Le client exsiste déja")]
        public string Lastname { get; set; }

        [DisplayName("Prenom")]
        [Required(ErrorMessage = "Le prenom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        [Remote(action: "VerifyName", controller: "Customers", AdditionalFields ="Lastname, Id", ErrorMessage = "Le client exsiste déja")]
        public string Firstname { get; set; }

        [DisplayName("Adresse")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Address { get; set; }

        [DisplayName("Téléphone (Prefix Obligatoire)")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?)(\d{2,4})(\s?)(\-?)((\(0\))?)(\s?)(\d{2})(\s?)(\-?)(\d{3})(\s?)(\-?)(\d{2})(\s?)(\-?)(\d{2})", ErrorMessage = "Le numero n'est pas valide")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Le format email n'est pas valide")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Email { get; set; }

        [DisplayName("Mobile (Prefix Obligatoire)")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+?)(\d{2,4})(\s?)(\-?)((\(0\))?)(\s?)(\d{2})(\s?)(\-?)(\d{3})(\s?)(\-?)(\d{2})(\s?)(\-?)(\d{2})", ErrorMessage = "Le numero n'est pas valable")]
        public string Mobile { get; set; }

        [DisplayName("Ville")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petit que 50 caracters")]
        public virtual Cities City { get; set; }
    }
}



