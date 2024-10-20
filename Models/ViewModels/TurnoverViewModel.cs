namespace Task2.Models.ViewModels
{
    public class TurnoverViewModel
    {
        public string AccountNumber { get; set; }
        //public string AccountClassName { get; set; } // Из таблицы AccountClass
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal ClosingBalanceDebit { get; set; }
        public decimal ClosingBalanceCredit { get; set; }
    }
}
