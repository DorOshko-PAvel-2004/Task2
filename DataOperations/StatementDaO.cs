using NPOI.SS.Formula.Functions;
using System.Data.Entity;
using Task2.Models.Entities;

namespace Task2.DataOperations
{
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
        public void Create(Statement statement)
        {
            context.Statements.Add(statement);
            context.SaveChanges();
        }
        public bool Contains(Statement statement)
        {
            return context.Statements.Any(x=>x.StatementName == statement.StatementName);
        }

        public int ReturnId(string statementName)
        {
            return context.Statements.First(x => x.StatementName == statementName).StatementId;
        }
        public IEnumerable<Statement>? GetStatements()
        {
            var statements = context.Statements.ToList() ?? null;
            foreach (var statement in statements)
            {
                context.Entry(statement).Reference("Bank").Load();
            }
            return statements;
        }
        public IEnumerable<Statement>? GetStatements(int bankId)
        {
            return context.Statements.Where(x => x.BankId == bankId) ?? null;
        }
        public Statement? GetStatement(int id)
        {
            Statement? statement = context.Statements.First(x=>x.StatementId==id) ?? null;
            context.Entry(statement).Reference("Bank").Load();
            return statement;
        }
        public Statement? GetStatement(string statementName)
        {
            return context.Statements.Where(x => x.StatementName == statementName)?.First() ?? null;
        }
        public void Update(Statement statement)
        {
            context.Statements.Update(statement);
            context.SaveChanges();

        }
        public void Delete(Statement? statement)
        {
            context.Statements.Remove(statement);
            context.SaveChanges();
        }
    }
}
