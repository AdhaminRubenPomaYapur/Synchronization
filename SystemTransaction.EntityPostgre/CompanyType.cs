using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class CompanyType
{
    public int CompanyTypeId { get; set; }

    public string CompanyTypeName { get; set; } = null!;

    public string CompanyTypeDescription { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; } = new List<Company>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();
}
