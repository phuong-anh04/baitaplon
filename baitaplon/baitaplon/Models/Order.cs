using System;
using System.Collections.Generic;

namespace baitaplon.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? Orderid { get; set; }

    public string? Status { get; set; }

    public string? PaymentType { get; set; }

    public int? UserId { get; set; }

    public DateTime? Date { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual Account? User { get; set; }
}
