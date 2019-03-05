using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Renteditems
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ContractId { get; set; }
        public int DurationId { get; set; }
        public int CategoryId { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public int? Linenb { get; set; }
        public byte? Partialreturn { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Contracts Contract { get; set; }
        public virtual Durations Duration { get; set; }
        public virtual Items Item { get; set; }
    }
}
