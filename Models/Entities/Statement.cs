using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.Models.Entities;

//Класс Statement - сущность отчёта как цельного документа
public partial class Statement
{
    //Первичный ключ
    public int StatementId { get; set; }
    //Название отчёта
    public string? StatementName { get; set; }
    //Дата создания отчёта
    public DateTime CreationDate { get; set; }
    //Валюта оборотов отчёта
    public string InCurrency { get; set; } = null!;
    //Внешний ключ. Ключ банка, которому отчёт принадлежит
    public int BankId { get; set; }
    //Bank - Statement. Связь one-to-many
    public virtual Bank Bank { get; set; } = null!;
    //Statement - Turnover. Связь one-to-many
    public virtual ICollection<Turnover> Turnovers { get; set; } = new List<Turnover>();
}
