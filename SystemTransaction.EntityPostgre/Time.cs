using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Time
{
    public int TimeId { get; set; }

    public int Day { get; set; }

    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public int? OfficeId { get; set; }

    public int? TimeState { get; set; }

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual Office? Office { get; set; }
}
