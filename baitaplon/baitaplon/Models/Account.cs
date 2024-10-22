using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace baitaplon.Models;

public partial class Account
{
    public int Id { get; set; }

    [Column("user_name")]
    public string UserName { get; set; } = null!;

    [Column("full_name")]
    public string? FullName { get; set; }

    public int? Gender { get; set; }

    public string? Image { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public int? Role { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
