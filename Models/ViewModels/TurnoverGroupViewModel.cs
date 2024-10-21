namespace Task2.Models.ViewModels
{
    //Модель хранения записей, сгруппированных по классам, для вывода на сраницу пользователю
    public class TurnoverGroupViewModel
    {
        public string AccountClassName { get; set; } // Название класса банковского счета
        public List<TurnoverSubGroupViewModel> SubGroups { get; set; }
    }
    //Модель хранения записей по подгруппам
    public class TurnoverSubGroupViewModel
    {
        public string SubGroup { get; set; } // Первые 2 цифры банковского счета (подгруппа)
        public List<TurnoverViewModel> Turnovers { get; set; } // Список оборотов для данной подгруппы
    }
}
