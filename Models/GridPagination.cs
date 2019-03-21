using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coliks.Models
{
    public class GridPagination
    {
        public int CurrentPage { get; set; }
        public double TotalPage { get; set; } // Buttons  
        public int TotalData { get; set; } // Total count of the filtered data  
        public List<Items> Data { get; set; }
        public int TakeCount { get; set; } = 100; // By default i am using 100 data per page  
        public FilterItems filters { get; set; } = new FilterItems(); // Search keys and value 
    }
}
