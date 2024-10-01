using System;
using System.Collections.Generic;

#nullable disable

namespace dashboard.Models
{
    public partial class Catg
    {
        public Catg()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Namee { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
