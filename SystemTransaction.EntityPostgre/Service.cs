using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public int ServiceState { get; set; }

    public int? CompanyTypeId { get; set; }

    public virtual CompanyType? CompanyType { get; set; }

    public virtual ICollection<Job> Jobs { get; } = new List<Job>();

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();
}
