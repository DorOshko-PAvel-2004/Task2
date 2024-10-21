using System;
using System.Collections.Generic;

namespace Task2.Models.Entities;

//Класс Bank - сущность, хранящая данные о банке
public partial class Bank
{
    //Первичный ключ
    public int BankId { get; set; }
    //Название банка
    public string BankName { get; set; } = null!;
    //Счета данного банка
    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    //Отчёты банка
    public virtual ICollection<Statement> Statements { get; set; } = new List<Statement>();
}
