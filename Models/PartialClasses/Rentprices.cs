using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    [ModelMetadataType(typeof(RentpricesMetadata))]
    public partial class Rentprices
    {
        
    }

    public class RentpricesMetadata
    {
        [DisplayName("Prix")]
        public int? Price { get; set; }
        [DisplayName("Catégorie")]
        public virtual Categories Category { get; set; }
        [DisplayName("Durée")]
        public virtual Durations Duration { get; set; }
        [DisplayName("Type d'équipement")]
        public virtual Geartypes Geartype { get; set; }
    }
}