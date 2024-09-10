using System;
using System.Collections.Generic;

namespace login.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();
}
