using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class SynchronizationCloud
{
    public int Synchronizationcloudid { get; set; }

    public int Transactioncloudid { get; set; }

    public DateTime Datetimelastupdate { get; set; }
}
