using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

public partial class Bank
{
    public int BankId { get; set; }

    public string BankName { get; set; } = null!;

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<Statement> Statements { get; set; } = new List<Statement>();
}
