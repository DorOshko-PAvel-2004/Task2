using NPOI.SS.Formula.Functions;
using System.Data.Entity;
using Task2.Models.Entities;

namespace Task2.DataOperations
{
    //Класс для работы с объектом класса Statement и бд
    public class StatementDaO
    {

        TaskDbContext? context;
        public StatementDaO()
        {

        }
        public StatementDaO(TaskDbContext context)
        {
            this.context = context;
        }
        //Добавление объекта в бд
        public void Create(Statement statement)
        {
            context.Statements.Add(statement);
            context.SaveChanges();
        }
        //Проверка на наличие объекта по назвнанию и по ключу банка
        public bool Contains(Statement statement)
        {
            return context.Statements.Any(x=>x.StatementName == statement.StatementName && x.BankId==statement.BankId);
        }
        //Получение значения первичного ключа по названию отчёта и названию банка
        public int ReturnId(string statementName,int bankId)
        {
            return context.Statements.Where(x => x.StatementName == statementName && x.BankId == bankId).First().StatementId;
        }
        //Получение списка всех отчётов из бд
        public IEnumerable<Statement>? GetStatements()
        {
            var statements = context.Statements.ToList() ?? null;
            foreach (var statement in statements)
            {
                //Явная загрузка внешних ключей каждому объекту
                context.Entry(statement).Reference("Bank").Load();
            }
            return statements;
        }
        //Получение отчёта по первичному ключу
        public Statement? GetStatement(int id)
        {
            Statement? statement = context.Statements.First(x=>x.StatementId==id) ?? null;
            //Явная загрузка значений внешних ключей объекту
            context.Entry(statement).Reference("Bank").Load();
            return statement;
        }
        //НЕ ИСПОЛЬЗУЕТСЯ
        //public IEnumerable<Statement>? GetStatements(int bankId)
        //{
        //    return context.Statements.Where(x => x.BankId == bankId) ?? null;
        //}
        //public Statement? GetStatement(string statementName)
        //{
        //    return context.Statements.Where(x => x.StatementName == statementName)?.First() ?? null;
        //}
        //public void Update(Statement statement)
        //{
        //    context.Statements.Update(statement);
        //    context.SaveChanges();

        //}
        //public void Delete(Statement? statement)
        //{
        //    context.Statements.Remove(statement);
        //    context.SaveChanges();
        //}
    }
}
