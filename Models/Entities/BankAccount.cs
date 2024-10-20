using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

public partial class BankAccount
{
    public int BankAccountId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public int BankId { get; set; }

    public int AccountClassId { get; set; }

    public virtual AccountClass AccountClass { get; set; } = null!;

    public virtual Bank Bank { get; set; } = null!;

    public virtual ICollection<Turnover> Turnovers { get; set; } = new List<Turnover>();
}
