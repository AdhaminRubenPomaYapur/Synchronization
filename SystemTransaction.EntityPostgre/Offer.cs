using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Offer
{
    public int OfferId { get; set; }

    public int OfficeId { get; set; }

    public int EmployeeId { get; set; }

    public int ServiceId { get; set; }

    public int? TimeId { get; set; }

    public int? Status { get; set; }

    public int? ClientId { get; set; }

    public string? DetailClient { get; set; }

    public string? DetailCompany { get; set; }

    public decimal? RateService { get; set; }

    public decimal? RateCustomer { get; set; }

    public int? WorkDayId { get; set; }

    public int? Stage { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Office Office { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Time? Time { get; set; }

    public virtual WorkDay? WorkDay { get; set; }
}
