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
            
            //�������� ������� ����������� �� ���� ������
            builder.Services.AddDbContextFactory<TaskDbContext>(options =>
            {
                //��������� ������ ����������� �� ����� ������������ appsettings.json
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                //��������� ��� ������������� ���� ������ Sql Server
                options.UseSqlServer(connectionString);
            });
            //���������� 1 ���������� ExcelService �� ���� ������
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
