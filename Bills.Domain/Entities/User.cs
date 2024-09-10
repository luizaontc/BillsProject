using System;
using System.Collections.Generic;

namespace Bills.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Document { get; set; }

    public bool? Status { get; set; }

    public int? Currency { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Currency? CurrencyNavigation { get; set; }
}
