using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    public partial class Customers
    {

        #region constructor

        public Customers()
        {
            Contracts = new HashSet<Contracts>();
            Purchases = new HashSet<Purchases>();
        }

        #endregion

        #region fields

        public int Id { get; set; }
        [DisplayName("Nom")]
        [Required (ErrorMessage ="Le nom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Lastname { get; set; }
        [DisplayName("Prenom")]
        [Required(ErrorMessage = "Le prenom du client est obligatoire")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Firstname { get; set; }
        [DisplayName("Adresse")]
        [StringLength(50, ErrorMessage ="La taille doit être plus petite que 50 caracters")]
        public string Address { get; set; }
        public int? CityId { get; set; }
        [DisplayName("Téléphone")]
        [Phone(ErrorMessage = "Le numero n'est pas valide")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage ="Le format email n'est pas valide")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petite que 50 caracters")]
        public string Email { get; set; }
        [DisplayName("Mobile")]
        [Phone(ErrorMessage = "Le numero n'est pas valide")]
        public string Mobile { get; set; }

        [DisplayName("Ville")]
        [StringLength(50, ErrorMessage = "La taille doit être plus petit que 50 caracters")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Contracts> Contracts { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }

        #endregion

        #region custom validation
        public class UniqueCustomer : ValidationAttribute
        {
            private String _firstName = string.Empty;
            private String _lastName = String.Empty;

            public UniqueCustomer(String lastName, String firstName)
            {
                _lastName = lastName;
                _firstName = firstName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                Customers customer = (Customers)validationContext.ObjectInstance;

                if ("" != null)
                {
                    return new ValidationResult(GetErrorMessage());
                }

                return ValidationResult.Success;
            }

            private string GetErrorMessage()
            {
                return $"Le client exsiste déja dans la base des données";
            }

        }


        #endregion
    }
}
