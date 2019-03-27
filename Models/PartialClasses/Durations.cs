using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    [ModelMetadataType(typeof(DurationMetadata))]
    public partial class Durations
    {
        
    }

    public class DurationMetadata
    {
        [DisplayName("Code")]
        public string Code { get; set; }
        [DisplayName("Détails")]
        public string Details { get; set; }
    }
}