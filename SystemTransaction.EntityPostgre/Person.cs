using System;
using System.Collections.Generic;

namespace SystemTransaction.EntityPostgre;

public partial class Person
{
    public int PersonId { get; set; }

    public string PersonName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public int Sex { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public int? AuthMethod { get; set; }

    public virtual ICollection<AccessToken> AccessTokens { get; } = new List<AccessToken>();

    public virtual ICollection<Client> Clients { get; } = new List<Client>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
