using Task2.Models.Entities;

namespace Task2.DataOperations
{
    public class BankAccountDaO
    {
        TaskDbContext? context;
        public BankAccountDaO()
        {

        }
        public BankAccountDaO(TaskDbContext context)
        {
            this.context = context;
        }
        public void Create(BankAccount accountClass)
        {
            context.BankAccounts.Add(accountClass);
            context.SaveChanges();
        }
        public bool ContainsInBank(string accountNumber, int bankId)
        {
            return context.BankAccounts.Any(x => x.BankId == bankId && x.AccountNumber == accountNumber);
        }
        public int ReturnId(string accountNumber)
        {
            return context.BankAccounts.First(x => x.AccountNumber == accountNumber).BankAccountId;
        }
        public IEnumerable<BankAccount>? GetBAccountClasses()
        {
            return context.BankAccounts ?? null;
        }
        public IEnumerable<BankAccount>? GetAccountsByClass(int accountClassId)
        {
            return context.BankAccounts.Where(x => x.AccountClassId == accountClassId) ?? null;
        }
        public BankAccount? GetAccountClass(int id)
        {
            return context.BankAccounts.Find(id) ?? null;
        }
        public void Update(BankAccount? account)
        {
            context.BankAccounts.Update(account);
            context.SaveChanges();

        }
        public void Delete(BankAccount? account)
        {
            context.BankAccounts.Remove(account);
            context.SaveChanges();
        }
    }
}
