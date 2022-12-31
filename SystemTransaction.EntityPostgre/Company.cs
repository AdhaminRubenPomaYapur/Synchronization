using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string CompanyWebPage { get; set; } = null!;

    public string CompanyLogo { get; set; } = null!;

    public int CompanyState { get; set; }

    public int? CompanyTypeId { get; set; }

    public int? CompanyCategory { get; set; }

    public virtual ICollection<Client> Clients { get; } = new List<Client>();

    public virtual CompanyType? CompanyType { get; set; }

    public virtual ICollection<CompanyWallet> CompanyWallets { get; } = new List<CompanyWallet>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Especiality> Especialities { get; } = new List<Especiality>();

    public virtual ICollection<Office> Offices { get; } = new List<Office>();
}
