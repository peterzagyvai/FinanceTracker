using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTrackerApi.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Data.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DebtEntity>()
                .HasOne(d => d.Creditor)
                .WithMany()
                .HasForeignKey(d => d.CreditorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DebtEntity>()
                .HasOne(d => d.Debtor)
                .WithMany()
                .HasForeignKey(d => d.DebtorId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<DebtEntity>()
                .OwnsOne(d => d.Loan, loan =>
                {
                    loan.Property(m => m.Amount).HasColumnName("Loan_Amount");
                    loan.Property(m => m.CurrencyISO).HasColumnName("Loan_CurrencyISO");
                });

            modelBuilder.Entity<ItemEntity>()
                .Property(i => i.UnitOfMeasure)
                .HasConversion<string>();

            modelBuilder.Entity<PurchasedItemEntity>()
                .OwnsOne(pi => pi.Price, price =>
                {
                    price.Property(m => m.Amount).HasColumnName("Price_Amount");
                    price.Property(m => m.CurrencyISO).HasColumnName("Price_Currency");
                });

            modelBuilder.Entity<TransactionEntity>()
                .OwnsOne(t => t.TransactionAmount, transaction =>
                {
                    transaction.Property(m => m.Amount).HasColumnName("Transaction_Amount");
                    transaction.Property(m => m.CurrencyISO).HasColumnName("Transaction_Currency");
                });

            modelBuilder.Entity<TransactionSourceEntity>()
                .HasOne(ts => ts.From)
                .WithMany()
                .HasForeignKey(ts => ts.FromId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionSourceEntity>()
                .HasOne(ts => ts.To)
                .WithMany()
                .HasForeignKey(ts => ts.ToId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DebtEntity> Debts => Set<DebtEntity>();
        public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
        public DbSet<IncomeEntity> Incomes => Set<IncomeEntity>();
        public DbSet<PurchaseEntity> Purchases => Set<PurchaseEntity>(); 
        public DbSet<PurchasedItemEntity> PurchasedItems => Set<PurchasedItemEntity>(); 
        public DbSet<ItemEntity> Items => Set<ItemEntity>();
        public DbSet<TransactionSourceEntity> TransactionSources => Set<TransactionSourceEntity>();
        public DbSet<TransactionEntity> Trasnactions => Set<TransactionEntity>();
        public DbSet<TransactionParticipantEntity> TransactionParticipants => Set<TransactionParticipantEntity>();
    }
}