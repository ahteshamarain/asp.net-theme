using System;
using System.Collections.Generic;

namespace login.Models;

public partial class Login
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Rid { get; set; }

    public virtual Role? RidNavigation { get; set; }
}
