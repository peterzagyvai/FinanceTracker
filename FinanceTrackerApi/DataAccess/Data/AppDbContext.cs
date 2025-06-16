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