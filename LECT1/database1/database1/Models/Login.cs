using System;
using System.Collections.Generic;

namespace database1.Models;

public partial class Login
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Roleid { get; set; }

    public virtual Role? Role { get; set; }
}
