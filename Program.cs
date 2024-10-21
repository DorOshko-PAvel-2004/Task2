using Microsoft.EntityFrameworkCore;
using Task2.Models.Entities;
using Task2.Services;

namespace Task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            //создание фабрики подключений на весь проект
            builder.Services.AddDbContextFactory<TaskDbContext>(options =>
            {
                //получение строки подключения из файла конфигурации appsettings.json
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                //Настройка для использования базы данных Sql Server
                options.UseSqlServer(connectionString);
            });
            //добавление 1 экземпляра ExcelService на весь проект
            builder.Services.AddSingleton<ExcelService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();
        }
    }
}
