using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

public partial class Turnover
{
    public int TurnoverId { get; set; }

    public int BankAccountId { get; set; }

    public int StatementId { get; set; }

    public decimal? OpeningBalanceDebit { get; set; }

    public decimal? OpeningBalanceCredit { get; set; }

    public decimal? TurnoverDebit { get; set; }

    public decimal? TurnoverCredit { get; set; }

    public decimal? ClosingBalanceDebit { get; set; }

    public decimal? ClosingBalanceCredit { get; set; }

    public virtual BankAccount BankAccount { get; set; } = null!;

    public virtual Statement Statement { get; set; } = null!;
}
