using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using Task2.DataOperations;
using Task2.Models.Entities;

namespace Task2.Services
{
    public class ExcelService
    {
        IDbContextFactory<TaskDbContext> contextFactory;
        AccountClassDao accountClassDaO;
        BankAccountDaO bankAccountDaO;
        BankDaO bankDaO;
        StatementDaO statementDaO;
        TurnoverDaO turnoverDaO;

        public ExcelService(IDbContextFactory<TaskDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void LoadInDatabase(Bank bank, Statement statement, List<AccountClass> accountClasses, 
            List<BankAccount> bankAccounts, List<Turnover> turnovers)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                accountClassDaO = new AccountClassDao(context);
                bankAccountDaO = new BankAccountDaO(context);
                bankDaO = new BankDaO(context);
                statementDaO = new StatementDaO(context);
                turnoverDaO = new TurnoverDaO(context);

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
                    int stagementId = statementDaO.ReturnId(statement.StatementName);

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
                            //есть ли первая цифра счёта не в названии класса
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
                            int accountId = bankAccountDaO.ReturnId(bankAccounts[j].AccountNumber);
                            if (!turnoverDaO.ContainsInStatement(accountId, stagementId))
                            {
                                turnovers[j].BankAccountId = accountId;
                                turnovers[j].StatementId = stagementId;
                                turnoverDaO.Create(turnovers[j]);
                            }
                        }
                        
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<Statement> GetStatements()
        {
            try
            {
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
        public Dictionary<Statement, List<Turnover>> ReturnFromDatabase(int statementId)
        {
            Dictionary<Statement, List<Turnover>> result;
            using (var context = contextFactory.CreateDbContext())
            {
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
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return null;
        }
    }
}
