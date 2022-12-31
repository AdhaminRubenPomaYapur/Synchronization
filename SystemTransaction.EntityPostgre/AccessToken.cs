using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class AccessToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime ExpirationTime { get; set; }

    public virtual Person User { get; set; } = null!;
}
