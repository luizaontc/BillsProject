using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bills.Domain.Entities;

public partial class InstallmentBill
{
    public int Id { get; set; }

    public long? BillsId { get; set; }

    public int? InstallmentNumber { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool? Status { get; set; }
    [JsonIgnore]
    public virtual Bill? Bills { get; set; }
}
