using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace coliks.Models
{
    [ModelMetadataType(typeof(CustomerCategoriesMetadata))]
    public partial class CustomerCategories
    {
        [NotMapped]
        [DisplayName("Nb de clients")]
        public int? totalCustomerCategories { get; set; }
    }

    public class CustomerCategoriesMetadata
    {
        [DisplayName("Valeur d'achat")]
        public int? Totalamount { get; set; }
        [DisplayName("Nom de lal categorie")]
        public string Categoryname { get; set; }
    }
}
