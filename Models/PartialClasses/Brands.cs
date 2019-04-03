using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    [ModelMetadataType(typeof(BrandsMetaData))]
    public partial class Brands
    {
    }

    public class BrandsMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Nom de la marque")]
        public string Brandname { get; set; }
    }
}
