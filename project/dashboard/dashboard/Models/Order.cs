using System;
using System.Collections.Generic;

#nullable disable

namespace dashboard.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? Userid { get; set; }
        public int? TotalAmount { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }

        public virtual Login User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
