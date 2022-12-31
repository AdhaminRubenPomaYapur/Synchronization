using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class SynchronizationLocalCompany
{
    public int Synchronizationlocalcompanyid { get; set; }

    public int Transactionlocalid { get; set; }

    public DateTime Datetimelastupdate { get; set; }
}
