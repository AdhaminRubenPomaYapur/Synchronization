using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class CompanyWalletTransaction
{
    public int CompanyWalletTransactionId { get; set; }

    public DateTime TrxDate { get; set; }

    public string TrxDetail { get; set; } = null!;

    public bool Debit { get; set; }

    public bool Credit { get; set; }

    public double Balance { get; set; }

    public int CompanyWalletId { get; set; }

    public virtual CompanyWallet CompanyWallet { get; set; } = null!;
}
