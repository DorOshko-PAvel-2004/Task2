using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

//Класс Turnover - модель записи оборота
public partial class Turnover
{
    //Первичный ключ
    public int TurnoverId { get; set; }
    //Внешний ключ. Ключ б/сч записи
    public int BankAccountId { get; set; }
    //Внешний ключ. Ключ отчёта, в котором хранится запись
    public int StatementId { get; set; }
    //Актив входящего сальжо
    public decimal? OpeningBalanceDebit { get; set; }
    //Пассив входящего сальдо
    public decimal? OpeningBalanceCredit { get; set; }
    //Дебет оборота
    public decimal? TurnoverDebit { get; set; }
    //Кредит оборота
    public decimal? TurnoverCredit { get; set; }
    //Актив выходящего сальдо
    public decimal? ClosingBalanceDebit { get; set; }
    //Пассив ввыходящего сальдо
    public decimal? ClosingBalanceCredit { get; set; }
    //BankAccount - Turnover. Связь one-to-many
    public virtual BankAccount BankAccount { get; set; } = null!;
    //Statement - Turnover. Связь one-to-many
    public virtual Statement Statement { get; set; } = null!;
}
