using System;
using System.Collections.Generic;

namespace database1.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Rname { get; set; }

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();
}
