using System;
using System.Collections.Generic;

namespace baitaplon.Models;

public partial class Product
{
    public int Pid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public int? Quantity { get; set; }

    public double? Discount { get; set; }

    public string? Image { get; set; }

    public int? Cid { get; set; }

    public virtual Category? CidNavigation { get; set; }
}
