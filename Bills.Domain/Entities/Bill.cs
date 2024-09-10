using System;
using System.Collections.Generic;

namespace Bills.Domain.Entities;

public partial class Bill
{
    public long Id { get; set; }

    public string? BillsName { get; set; }

    public string? Description { get; set; }

    public DateOnly DueDate { get; set; }

    public int? Installments { get; set; }
    public int? InstallmentsNumber { get; set; }

    public decimal? Amount { get; set; }

    public int? UserId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<InstallmentBill> InstallmentBills { get; set; } = new List<InstallmentBill>();

    public virtual User? User { get; set; }
}
