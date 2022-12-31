using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Client
{
    public int ClientId { get; set; }

    public int ClientState { get; set; }

    public int CompanyId { get; set; }

    public int PersonId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; } = new List<Offer>();

    public virtual Person Person { get; set; } = null!;
}
