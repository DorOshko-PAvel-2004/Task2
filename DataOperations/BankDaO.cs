﻿using Task2.Models.Entities;

namespace Task2.DataOperations
{
    //класс для работы с объектом класса Bank и бд
    public class BankDaO
    {
        TaskDbContext? context;
        public BankDaO()
        {

        }
        public BankDaO(TaskDbContext context)
        {
            this.context = context;
        }
        //Добавление объекта в бд
        public void Create(Bank bank)
        {
            context.Banks.Add(bank);
            context.SaveChanges();
        }
        //Проверка наличия банка по его названию
        public bool Contains(Bank bank)
        {
            return context.Banks.Any(x => x.BankName == bank.BankName);
        }
        //Возврат значения первичного ключа
        public int ReturnId(string bankName)
        {
            return context.Banks.First(x => x.BankName == bankName).BankId;
        }
         //НЕ ИСПОЛЬЗУЕТСЯ
        //public IEnumerable<Bank>? GetBanks()
        //{
        //    return context.Banks ?? null;
        //}
        //public Bank? GetBank(int id)
        //{
        //    return context.Banks.Find(id) ?? null;
        //}
        //public Bank? GetBank(string bankName)
        //{
        //    return context.Banks.Where(x=>x.BankName==bankName)?.First() ?? null;
        //}
        //public void Update(Bank bank) 
        //{
        //    context.Banks.Update(bank);
        //    context.SaveChanges();

        //}
        //public void Delete(Bank? bank) 
        //{
        //    context.Banks.Remove(bank);
        //    context.SaveChanges();
        //}
    }
}
