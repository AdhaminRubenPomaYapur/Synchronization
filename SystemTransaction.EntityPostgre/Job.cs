using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Job
{
    public int JobId { get; set; }

    public int EspecialityId { get; set; }

    public int ServiceId { get; set; }

    public virtual Especiality Especiality { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
