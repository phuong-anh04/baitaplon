using System;
using System.Collections.Generic;

namespace baitaplon.Models;

public partial class Category
{
    public int Cid { get; set; }

    public string? Name { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
