using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    [ModelMetadataType(typeof(CustomerCategoriesMetadata))]
    public partial class CustomerCategories
    {
    }

    public class CustomerCategoriesMetadata
    {
        [DisplayName("Valeur d'achat")]
        public int? Totalamount { get; set; }
        [DisplayName("Nom de lal categorie")]
        public string Categoryname { get; set; }
    }
}
