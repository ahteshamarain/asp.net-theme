using System;
using System.Collections.Generic;

namespace database1.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Catname { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
