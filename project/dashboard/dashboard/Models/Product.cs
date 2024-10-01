using System;
using System.Collections.Generic;

#nullable disable

namespace dashboard.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Namee { get; set; }
        public int? Price { get; set; }
        public string Picture { get; set; }
        public string Descp { get; set; }
        public int? CatId { get; set; }
        public int? Qty { get; set; }

        public virtual Catg Cat { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
