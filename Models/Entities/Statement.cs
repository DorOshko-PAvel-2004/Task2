using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.Models.Entities;

public partial class Statement
{
    public int StatementId { get; set; }

    public string? StatementName { get; set; }

    public DateTime CreationDate { get; set; }

    public string InCurrency { get; set; } = null!;
    [ForeignKey("Bank")]
    public int BankId { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public virtual ICollection<Turnover> Turnovers { get; set; } = new List<Turnover>();
}
