using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    [ModelMetadataType(typeof(ItemsMetaData))]
    public partial class Items
    {
    }

    public class ItemsMetaData
    {
        [Display(Name = "No. article")]
        [Required(ErrorMessage = "Le numéro d'article doit être défini")]
        [Remote("IsItemNbExist", "Items", AdditionalFields = "Id",
                ErrorMessage = "Ce numéro d'article existe déjà !")]
        public string Itemnb { get; set; }

        [Display(Name = "Marque")]
        public string Brand { get; set; }

        [Display(Name = "Modèle")]
        public string Model { get; set; }

        [Display(Name = "Taille")]
        public int? Size { get; set; }

        [Display(Name = "Coût")]
        public int? Cost { get; set; }

        [Display(Name = "Retourné")]
        public int? Returned { get; set; }

        public string Type { get; set; }

        public int? Stock { get; set; }

        [Display(Name = "Numéro de série")]
        public string Serialnumber { get; set; }

        [Display(Name = "Catégorie")]
        public Categories Category { get; set; }

        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }
    }
}
