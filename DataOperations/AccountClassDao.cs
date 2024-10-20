using Task2.Models.Entities;

namespace Task2.DataOperations
{
    public class AccountClassDao
    {
        TaskDbContext? context;
        public AccountClassDao()
        {

        }
        public AccountClassDao(TaskDbContext context)
        {
            this.context = context;
        }
        public void Create(AccountClass accountClass)
        {
            context.AccountClasses.Add(accountClass);
            int a = context.SaveChanges();
        }
        public bool Contains(string accountClassName)
        {
            return context.AccountClasses.Any(x=>x.AccountClassName == accountClassName);
        }
        public int ReturnId(string accountClassName)
        {
            return context.AccountClasses.First(x => x.AccountClassName == accountClassName).AccountClassId;
        }
        public IEnumerable<AccountClass>? GetBAccountClasses()
        {
            return context.AccountClasses ?? null;
        }
        public AccountClass? GetAccountClass(int id)
        {
            return context.AccountClasses.Find(id) ?? null;
        }
        public AccountClass? GetAccountClass(string accountClassName)
        {
            return context.AccountClasses.Where(x => x.AccountClassName == accountClassName)?.First() ?? null;
        }
        public void Update(AccountClass? accountClass)
        {
            context.AccountClasses.Update(accountClass);
            context.SaveChanges();

        }
        public void Delete(AccountClass? accountClass)
        {
            context.AccountClasses.Remove(accountClass);
            context.SaveChanges();
        }
    }
}
