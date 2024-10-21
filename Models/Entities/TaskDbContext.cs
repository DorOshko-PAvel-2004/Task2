using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task2.Models.Entities;

//Класс контескта вззаимодействия с бд 
//Работа происходит через ORM, используя EntityFrameworkCore
public partial class TaskDbContext : DbContext
{
    public TaskDbContext()
    {
    }

    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<AccountClass> AccountClasses { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Statement> Statements { get; set; }

    public virtual DbSet<Turnover> Turnovers { get; set; }

    //Метод настройки конфигурации. Работает при неинициализированной фабрике
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)//Аварийный случай
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-93G6P48\\USEFUL;Initial Catalog=B1Task2;Integrated Security=True;Encrypt=False");
        }
    }
    //Метод создания моделей в ORM и настройка связей между ними
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountClass>(entity =>
        {
            entity.ToTable("AccountClass");

            entity.Property(e => e.AccountClassId).HasColumnName("AccountClassID");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__Bank__AA08CB336BF9B95F");

            entity.ToTable("Bank");

            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.BankName).HasMaxLength(100);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.BankAccountId).HasName("PK__BankAcco__4FC8E7412834D653");

            entity.ToTable("BankAccount");

            entity.HasIndex(e => new { e.AccountNumber, e.BankId }, "BancAccountBank").IsUnique();

            entity.Property(e => e.BankAccountId).HasColumnName("BankAccountID");
            entity.Property(e => e.AccountClassId).HasColumnName("AccountClassID");
            entity.Property(e => e.AccountNumber).HasMaxLength(4);
            entity.Property(e => e.BankId).HasColumnName("BankID");

            entity.HasOne(d => d.AccountClass).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.AccountClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankAccou__Accou__35BCFE0A");

            entity.HasOne(d => d.Bank).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankAccou__BankI__34C8D9D1");
        });

        modelBuilder.Entity<Statement>(entity =>
        {
            entity.HasKey(e => e.StatementId).HasName("PK__Statemen__2B7E042260377691");

            entity.ToTable("Statement");

            entity.Property(e => e.StatementId).HasColumnName("StatementID");
            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.InCurrency).HasMaxLength(20);
            entity.Property(e => e.StatementName).HasMaxLength(150);

            entity.HasOne(d => d.Bank).WithMany(p => p.Statements)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statement__BankI__173876EA");
        });

        modelBuilder.Entity<Turnover>(entity =>
        {
            entity.HasKey(e => e.TurnoverId).HasName("PK__Turnover__F0EEF893A48E21DC");

            entity.ToTable("Turnover");

            entity.Property(e => e.TurnoverId).HasColumnName("TurnoverID");
            entity.Property(e => e.BankAccountId).HasColumnName("BankAccountID");
            entity.Property(e => e.ClosingBalanceCredit)
                .HasComputedColumnSql("(([OpeningBalanceCredit]+[TurnoverCredit])-[TurnoverDebit])", true)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.ClosingBalanceDebit)
                .HasComputedColumnSql("(([OpeningBalanceDebit]+[TurnoverDebit])-[TurnoverCredit])", true)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.OpeningBalanceCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalanceDebit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StatementId).HasColumnName("StatementID");
            entity.Property(e => e.TurnoverCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TurnoverDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BankAccount).WithMany(p => p.Turnovers)
                .HasForeignKey(d => d.BankAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Turnover__BankAc__3B75D760");

            entity.HasOne(d => d.Statement).WithMany(p => p.Turnovers)
                .HasForeignKey(d => d.StatementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Turnover__Statem__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
