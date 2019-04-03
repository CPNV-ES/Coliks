using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coliks.Models
{
    public class FilterItems
    {
        public string itemnb { get; set; }
        public int? brand { get; set; }
        public string model { get; set; }
        public Nullable<int> size { get; set; } = null;
        public Nullable<int> stock { get; set; } = null;
        public int? category { get; set; }
        public string search { get; set; }
        //public string filter { get; set; }
        //public string sortExpression { get; set; } = "Itemnb";
        //public int page { get; set; } = 1;


    }
}
