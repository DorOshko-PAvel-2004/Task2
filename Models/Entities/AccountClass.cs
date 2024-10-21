using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task2.Models.Entities;
//класс AccountClass - сущность, хранящая данные о классах отчётов
public partial class AccountClass
{
    //Первичный ключ
    public int AccountClassId { get; set; }
    //Название класса
    public string AccountClassName { get; set; } = null!;
    //Привязанные к классу б/сч
    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
