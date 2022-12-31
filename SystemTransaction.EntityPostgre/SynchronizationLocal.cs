using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class SynchronizationLocal
{
    public int Synchronizationlocalid { get; set; }

    public int Transactionlocalid { get; set; }

    public DateTime Datetimelastupdate { get; set; }
}
