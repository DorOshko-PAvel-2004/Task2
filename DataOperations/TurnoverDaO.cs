using System.Data.Entity;
using Task2.Models.Entities;

namespace Task2.DataOperations
{
    //класс для взаимодействия с объектом класса Turnover и бд
    public class TurnoverDaO
    {
        TaskDbContext? context;
        public TurnoverDaO()
        {

        }
        public TurnoverDaO(TaskDbContext context)
        {
            this.context = context;
        }
        //Добавление объекта в бд
        public void Create(Turnover turnover)
        {
            context.Turnovers.Add(turnover);
            context.SaveChanges();
        }
        //Проверка на наличие записи оборота по ключу отчёта и б/сч
        public bool ContainsInStatement(int accountId, int statementId)
        {
            return context.Turnovers.Any(x=>x.StatementId==statementId && x.BankAccountId==accountId);
        }
        //Получение записей оборота для одного конкретного отчёта
        public IEnumerable<Turnover>? GetTurnoversByStatement(int statementId)
        {
            var turnovers = context.Turnovers.Where(x => x.StatementId == statementId).ToList();
            foreach(var turnover in turnovers)
            {
                context.Entry(turnover).Reference("BankAccount").Load();
                context.Entry(turnover.BankAccount).Reference("AccountClass").Load();
            }
            return turnovers;
        }
        //НЕ ИСПОЛЬЗУЕТСЯ
        //public void CreateMany(IEnumerable<Turnover> turnovers)
        //{
        //    context.Turnovers.AddRange(turnovers);
        //    context.SaveChanges();
        //}
        //public IEnumerable<Turnover>? GetTurnovers()
        //{
        //    return context.Turnovers ?? null;
        //}
        //public Turnover? GetTurnoverById(int id)
        //{
        //    return context.Turnovers.Find(id) ?? null;
        //}
        //public Turnover GetTurnoverByAccount(int accountId)
        //{
        //    return context.Turnovers.Where(x => x.BankAccountId == accountId)?.First() ?? null;
        //}
        //public void Update(Turnover turnover)
        //{
        //    context.Turnovers.Update(turnover);
        //    context.SaveChanges();
        //}
        //public void Delete(Turnover? turnover)
        //{
        //    context.Turnovers.Remove(turnover);
        //    context.SaveChanges();
        //}
    }
}
