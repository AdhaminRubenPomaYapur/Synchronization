using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class SynchronizationCloudCompany
{
    public int Synchronizationcloudcompanyid { get; set; }

    public int Transactioncloudid { get; set; }

    public DateTime Datetimelastupdate { get; set; }
}
