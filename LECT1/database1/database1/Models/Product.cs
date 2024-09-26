using System;
using System.Collections.Generic;

namespace database1.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Proname { get; set; }

    public string? Proimage { get; set; }

    public string? Prodes { get; set; }

    public int Catid { get; set; }

    public virtual Category Cat { get; set; } = null!;
}
