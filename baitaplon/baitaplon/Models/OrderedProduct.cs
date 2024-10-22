using System;
using System.Collections.Generic;

namespace baitaplon.Models;

public partial class OrderedProduct
{
    public int Oid { get; set; }

    public string? Name { get; set; }

    public int? Quantity { get; set; }

    public string? Price { get; set; }

    public string? Image { get; set; }

    public int? Orderid { get; set; }

    public virtual Order? Order { get; set; }
}
