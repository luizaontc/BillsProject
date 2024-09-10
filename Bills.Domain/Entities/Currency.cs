using System;
using System.Collections.Generic;

namespace Bills.Domain.Entities;

public partial class Currency
{
    public int Id { get; set; }

    public string? NameEn { get; set; }

    public string? NameBr { get; set; }

    public string? Symbol { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
