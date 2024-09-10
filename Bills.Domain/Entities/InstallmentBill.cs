using System;
using System.Collections.Generic;

namespace Bills.Domain.Entities;

public partial class InstallmentBill
{
    public int Id { get; set; }

    public long? BillsId { get; set; }

    public int? InstallmentNumber { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool? Status { get; set; }

    public virtual Bill? Bills { get; set; }
}
