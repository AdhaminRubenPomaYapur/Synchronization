using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Position
{
    public int PositionId { get; set; }

    public int EspecialityId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Especiality Especiality { get; set; } = null!;
}
