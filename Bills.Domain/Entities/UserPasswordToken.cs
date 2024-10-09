using System;
using System.Collections.Generic;

namespace Bills.Domain.Entities;

public partial class UserPasswordToken
{
    public long IdPasswordToken { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresIn { get; set; }

    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
