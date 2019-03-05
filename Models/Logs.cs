using System;
using System.Collections.Generic;

namespace coliks.Models
{
    public partial class Logs
    {
        public int Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Text { get; set; }
    }
}
