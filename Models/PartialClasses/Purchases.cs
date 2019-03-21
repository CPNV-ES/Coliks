using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    public partial class Purchases
    {
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Description")]
        [Required(ErrorMessage = "La description est obligatoire")]
        public string Description { get; set; }
        [DisplayName("Total")]
        [Required(ErrorMessage = "Le montant est vide")]
        public double? Amount { get; set; }
    }
}
