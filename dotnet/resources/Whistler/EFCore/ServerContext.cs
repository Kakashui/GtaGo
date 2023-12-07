using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Whistler.MoneySystem.Models;

namespace Whistler.EFCore
{
    class ServerContext : DbContext
    {
        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<CreditModel> Credits { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("WhistlerConnection"); //"Server=localhost;uid=root;pwd=root;Port=3306;database=whistler;";//
            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CreditModel>()
                .Property(credit => credit.HistoryPayment)
                .HasConversion(
                    history => JsonConvert.SerializeObject(history),
                    history => JsonConvert.DeserializeObject<List<CreditPayment>>(history)
                );
        }
    }
}
