using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2.Models.Entities;

public partial class AccountClass
{
    public int AccountClassId { get; set; }

    public string AccountClassName { get; set; } = null!;

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
