using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    public partial class RentedItems
    {
        
    }

    public class RentedItemsMetadata
    {
        [DisplayName("Prix")]
        public int? Price { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Numéro de ligne")]
        public int? Linenb { get; set; }
        [DisplayName("Retour partiel")]
        public byte? Partialreturn { get; set; }
        [DisplayName("Catégorie")]
        public virtual Categories Category { get; set; }
        [DisplayName("Contrat")]
        public virtual Contracts Contract { get; set; }
        [DisplayName("Durée")]
        public virtual Durations Duration { get; set; }
        [DisplayName("Objet")]
        public virtual Items Item { get; set; }

    }
}