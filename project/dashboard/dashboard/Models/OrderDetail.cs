using System;
using System.Collections.Generic;

#nullable disable

namespace dashboard.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProId { get; set; }
        public string ProName { get; set; }
        public int? Qty { get; set; }
        public int? Proprice { get; set; }
        public string Date { get; set; }
        public int? Userid { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Pro { get; set; }
        public virtual Login User { get; set; }
    }
}
