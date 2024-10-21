using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

//класс BankAccount - сущность банковского счёта
public partial class BankAccount
{
    //Первичный ключ
    public int BankAccountId { get; set; }
    //Банковский счёт
    public string AccountNumber { get; set; } = null!;
    //Внешний ключ. Значение ключа банка, которому принадлежит счёт
    public int BankId { get; set; }
    //Внешний ключ. Значение ключа класса, к которому счёт относится 
    public int AccountClassId { get; set; }
    //AccountClass - BankAccount связь one-to-many
    public virtual AccountClass AccountClass { get; set; } = null!;
    //Bank - BankAccount связь one-to-many
    public virtual Bank Bank { get; set; } = null!;
    //Turnovers - BankAccount связь many-to-one
    public virtual ICollection<Turnover> Turnovers { get; set; } = new List<Turnover>();
}
