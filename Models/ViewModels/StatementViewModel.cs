namespace Task2.Models.ViewModels
{
    //Модель вывода данных отчёта на страницу пользователя
    public class StatementViewModel
    {
        public int StatementID { get; set; }
        public string StatementName { get; set; }
        public DateTime CreationDate { get; set; }
        public string InCurrency { get; set; }
        public string BankName { get; set; } // Из таблицы Bank
        public IEnumerable<TurnoverGroupViewModel> TurnoverGroups { get; set; }
    }
}
