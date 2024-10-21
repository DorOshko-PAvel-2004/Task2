using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using Task2.DataOperations;
using Task2.Models.Entities;

namespace Task2.Services
{
    //Сервис для работы с бд
    public class ExcelService
    {
        //Переменная фабрики для получения объекта TaskDbContext
        IDbContextFactory<TaskDbContext> contextFactory;
        //Объекты работы с бд
        AccountClassDao accountClassDaO;
        BankAccountDaO bankAccountDaO;
        BankDaO bankDaO;
        StatementDaO statementDaO;
        TurnoverDaO turnoverDaO;
        //получение объекта фабрики
        public ExcelService(IDbContextFactory<TaskDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        //Загрузка в бд отчёта
        public void LoadInDatabase(Bank bank, Statement statement, List<AccountClass> accountClasses, 
            List<BankAccount> bankAccounts, List<Turnover> turnovers)
        {
            //получение объекта TaskDbContext для подключения и работы с бд
            using (var context = contextFactory.CreateDbContext())
            {
                //создание объектов для работы с бд с передачей им объекта TaskDbContext при инициализации
                accountClassDaO = new AccountClassDao(context);
                bankAccountDaO = new BankAccountDaO(context);
                bankDaO = new BankDaO(context);
                statementDaO = new StatementDaO(context);
                turnoverDaO = new TurnoverDaO(context);

                //создание транзакции для единой записи в бд
                var transaction = context.Database.BeginTransaction();
                try
                {
                    if (!bankDaO.Contains(bank))
                    {
                        bankDaO.Create(bank);
                    }
                    
                    int bankId = bankDaO.ReturnId(bank.BankName);

                    statement.BankId = bankId;
                    if (!statementDaO.Contains(statement))
                    {
                        statementDaO.Create(statement);
                    }
                    int stagementId = statementDaO.ReturnId(statement.StatementName, bankId);
                    //переменная для просмотра всех значений коллекции bankAccounts и turnovers
                    int j = 0;
                    for (int i = 0; i < accountClasses.Count(); i++)
                    {
                        if (!accountClassDaO.Contains(accountClasses[i].AccountClassName))
                        {
                            accountClassDaO.Create(accountClasses[i]);
                        }
                            int accountClassId = accountClassDaO.ReturnId(accountClasses[i].AccountClassName);
                        for (; j < bankAccounts.Count(); j++)
                        {
                            //Проверка на наличии в названии класса первой цифры счёта
                            //Если нет, данный цикл прерывается, происходит переход к следующему классу
                            if (!accountClasses[i].AccountClassName.Contains(bankAccounts[j].AccountNumber.First()))
                            {
                                break;
                            }
                            if (!bankAccountDaO.ContainsInBank(bankAccounts[j].AccountNumber, bankId))
                            {
                                bankAccounts[j].AccountClassId = accountClassId;
                                bankAccounts[j].BankId = bankId;
                                bankAccountDaO.Create(bankAccounts[j]);
                            }
                            int accountId = bankAccountDaO.ReturnId(bankAccounts[j].AccountNumber, bankId);
                            if (!turnoverDaO.ContainsInStatement(accountId, stagementId))
                            {
                                turnovers[j].BankAccountId = accountId;
                                turnovers[j].StatementId = stagementId;
                                turnoverDaO.Create(turnovers[j]);
                            }
                        }
                        
                    }
                    //Подтверждение действий транзакции, освобождение ресурсов
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    //Откат всех выполненных действий транзакции
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        //получение списка отчётов из бд
        public List<Statement> GetStatements()
        {
            try
            {
                //Создание объекта контекста подключения к бд
                using (var context = contextFactory.CreateDbContext())
                {
                    statementDaO = new StatementDaO(context);
                    return statementDaO.GetStatements().ToList();
                }
            }
            catch(Exception ex)
            {
            return null;
            }
        }
        //Получение объекта отчета по значению ключа
        public Dictionary<Statement, List<Turnover>> ReturnFromDatabase(int statementId)
        {
            //Журнал отчёта. Ключ - головная часть отчёта, значение - список значений
            Dictionary<Statement, List<Turnover>> result;
            using (var context = contextFactory.CreateDbContext())
            {
                //начало транзакции
                var transaction = context.Database.BeginTransaction();
                try
                {
                    statementDaO = new StatementDaO(context);
                    turnoverDaO = new TurnoverDaO(context);
                    Statement statement = statementDaO.GetStatement(statementId);
                    List<Turnover> turnovers = turnoverDaO.GetTurnoversByStatement(statementId)?.ToList();
                    result = new Dictionary<Statement, List<Turnover>>
                    {
                        { statement, turnovers }
                    };
                    //подтверждение транзакции
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    //Откат транзакции
                    transaction.Rollback();
                }
            }
            return null;
        }
    }
}
