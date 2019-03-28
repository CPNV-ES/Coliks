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
        [DisplayName("Nom")]
        [Remote(action: "VerifyName", controller: "Customers", AdditionalFields = nameof(createLastname), ErrorMessage = "Le client exsiste déja")]
        [Required(ErrorMessage = "Le nom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string createLastname { get { return Lastname; } set { Lastname = value; } }
        [NotMapped]
        [Remote(action: "VerifyName", controller: "Customers", AdditionalFields = nameof(createFirstname), ErrorMessage = "Le client exsiste déja")]
        [DisplayName("Prenom")]
        [Required(ErrorMessage = "Le prenom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string createFirstname { get { return Firstname; } set { Firstname = value; } }

    }

    public class CustomersMetadata
    {
        [DisplayName("Nom")]
        [Required(ErrorMessage = "Le nom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Lastname { get; set; }
        [DisplayName("Prenom")]
        [Required(ErrorMessage = "Le prenom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Firstname { get; set; }
        [DisplayName("Adresse")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Address { get; set; }
        [DisplayName("Téléphone")]
        [Phone(ErrorMessage = "Le numero n'est pas valide")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Le format email n'est pas valide")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Email { get; set; }
        [DisplayName("Mobile")]
        [Phone(ErrorMessage = "Le numero n'est pas valide")]
        public string Mobile { get; set; }

        [DisplayName("Ville")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petit que 50 caracters")]
        public virtual Cities City { get; set; }
    }
}



