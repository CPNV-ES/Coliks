using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace coliks.Models
{
    [ModelMetadataType(typeof(ContractsMetadata))]
    public partial class Contracts
    {
        
    }

    public class ContractsMetadata
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Date de création")]
        public DateTime? Creationdate { get; set; }
        [DisplayName("Date de retour éffective")]
        public DateTime? Effectivereturn { get; set; }
        [DisplayName("Date de retour plannifiée")]
        public DateTime? Plannedreturn { get; set; }
        [DisplayName("Notes")]
        public string Notes { get; set; }
        [DisplayName("Prix total")]
        public double? Total { get; set; }
        [DisplayName("Date d'emprunt")]
        public DateTime? Takenon { get; set; }
        [DisplayName("Date de paiement")]
        public DateTime? Paidon { get; set; }
        [DisplayName("Assurance")]
        public byte? Insurance { get; set; }
        [DisplayName("Aller checher")]
        public byte? Goget { get; set; }
        [DisplayName("Client")]
        public virtual Customers Customer { get; set; }
        [DisplayName("Staff aidant")]
        public virtual Staffs HelpStaff { get; set; }
        [DisplayName("Staff tunant")]
        public virtual Staffs TuneStaff { get; set; }
    }
}