namespace Task2.Models.ViewModels
{
    //Модель хранения записи оборота для вывода на страницу пользователю
    public class TurnoverViewModel
    {
        public string AccountNumber { get; set; }
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal ClosingBalanceDebit { get; set; }
        public decimal ClosingBalanceCredit { get; set; }
    }
}
