using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Contracts
    {
        public Contracts()
        {
            Renteditems = new HashSet<Renteditems>();
        }

        public int Id { get; set; }
        public DateTime? Creationdate { get; set; }
        public DateTime? Effectivereturn { get; set; }
        public DateTime? Plannedreturn { get; set; }
        public int CustomerId { get; set; }
        public string Notes { get; set; }
        public double? Total { get; set; }
        public DateTime? Takenon { get; set; }
        public DateTime? Paidon { get; set; }
        public byte? Insurance { get; set; }
        public byte? Goget { get; set; }
        public int? HelpStaffId { get; set; }
        public int? TuneStaffId { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Staffs HelpStaff { get; set; }
        public virtual Staffs TuneStaff { get; set; }
        public virtual ICollection<Renteditems> Renteditems { get; set; }
    }
}
