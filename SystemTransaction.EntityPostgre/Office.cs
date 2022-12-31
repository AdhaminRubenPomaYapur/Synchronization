using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Office
{
    public int OfficeId { get; set; }

    public string OfficeName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public double GeoRefLatitude { get; set; }

    public double GeoRefLongitude { get; set; }

    public int OfficeState { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual ICollection<Time> Times { get; } = new List<Time>();

    public virtual ICollection<WorkDay> WorkDays { get; } = new List<WorkDay>();
}
