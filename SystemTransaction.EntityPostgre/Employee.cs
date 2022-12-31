using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int EmployeeState { get; set; }

    public int Role { get; set; }

    public int CompanyId { get; set; }

    public int PersonId { get; set; }

    public int? OfficeId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual Office? Office { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Position> Positions { get; } = new List<Position>();
}
