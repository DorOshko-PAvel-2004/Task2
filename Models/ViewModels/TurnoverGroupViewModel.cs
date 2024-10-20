namespace Task2.Models.ViewModels
{
    public class TurnoverGroupViewModel
    {
        public string AccountClassName { get; set; } // Название класса банковского счета
        public List<TurnoverSubGroupViewModel> SubGroups { get; set; }
    }
    public class TurnoverSubGroupViewModel
    {
        public string SubGroup { get; set; } // Первые 2 цифры банковского счета (подгруппа)
        public List<TurnoverViewModel> Turnovers { get; set; } // Список оборотов для данной подгруппы
    }
}
