using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Staffs
    {
        public Staffs()
        {
            ContractsHelpStaff = new HashSet<Contracts>();
            ContractsTuneStaff = new HashSet<Contracts>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Contracts> ContractsHelpStaff { get; set; }
        public virtual ICollection<Contracts> ContractsTuneStaff { get; set; }
    }
}
