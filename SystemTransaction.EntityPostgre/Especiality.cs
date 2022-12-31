using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Especiality
{
    public int EspecialityId { get; set; }

    public string EspecialityName { get; set; } = null!;

    public int EspecialityState { get; set; }

    public int CompanyId { get; set; }

    public string? EspecialityDescription { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; } = new List<Job>();

    public virtual ICollection<Position> Positions { get; } = new List<Position>();
}
