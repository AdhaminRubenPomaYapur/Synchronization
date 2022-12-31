using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public string Tablename { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public string Mdl { get; set; } = null!;

    public DateTime Datetime { get; set; }

    public int Officeid { get; set; }
}
