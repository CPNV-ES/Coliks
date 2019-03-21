using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace coliks.Models
{
    public partial class Customers
    {

        #region constructor

        public Customers()
        {
            Contracts = new HashSet<Contracts>();
            Purchases = new HashSet<Purchases>();
        }

        #endregion

        #region fields

        public int Id { get; set; }
        public int? CityId { get; set; }
       
        public virtual ICollection<Contracts> Contracts { get; set; }
        public virtual ICollection<Purchases> Purchases { get; set; }
        
        #endregion
    }

}



