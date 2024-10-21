using Task2.Models.Entities;

namespace Task2.DataOperations
{
    //класс работы с объектом класса AccountClass и бд
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
        //Добавление объекта в бд
        public void Create(AccountClass accountClass)
        {
            context.AccountClasses.Add(accountClass);
            int a = context.SaveChanges();
        }
        //Проверка наличия класса б/сч по названию
        public bool Contains(string accountClassName)
        {
            return context.AccountClasses.Any(x=>x.AccountClassName == accountClassName);
        }
        //Получение первичного ключа по названию
        public int ReturnId(string accountClassName)
        {
            return context.AccountClasses.First(x => x.AccountClassName == accountClassName).AccountClassId;
        }
        //НЕ ИСПОЛЬЗУЕТСЯ
        //public IEnumerable<AccountClass>? GetBAccountClasses()
        //{
        //    return context.AccountClasses ?? null;
        //}
        //public AccountClass? GetAccountClass(int id)
        //{
        //    return context.AccountClasses.Find(id) ?? null;
        //}
        //public AccountClass? GetAccountClass(string accountClassName)
        //{
        //    return context.AccountClasses.Where(x => x.AccountClassName == accountClassName)?.First() ?? null;
        //}
        //public void Update(AccountClass? accountClass)
        //{
        //    context.AccountClasses.Update(accountClass);
        //    context.SaveChanges();

        //}
        //public void Delete(AccountClass? accountClass)
        //{
        //    context.AccountClasses.Remove(accountClass);
        //    context.SaveChanges();
        //}
    }
}
