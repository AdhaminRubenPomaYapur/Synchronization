using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class WorkDay
{
    public int WorkDayId { get; set; }

    public DateTime Date { get; set; }

    public int Day { get; set; }

    public bool IsHoliday { get; set; }

    public bool IsWorkDay { get; set; }

    public string DetailDay { get; set; } = null!;

    public int? OfficeId { get; set; }

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual Office? Office { get; set; }
}
