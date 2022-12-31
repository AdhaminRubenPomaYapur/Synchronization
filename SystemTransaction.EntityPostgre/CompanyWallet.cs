using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class CompanyWallet
{
    public int CompanyWalletId { get; set; }

    public string Currency { get; set; } = null!;

    public double Balance { get; set; }

    public int CompanyWalletState { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CompanyWalletTransaction> CompanyWalletTransactions { get; } = new List<CompanyWalletTransaction>();
}
