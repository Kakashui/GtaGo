using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Whistler.Domain.Phone;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;
using Whistler.Domain.Phone.Taxi;
using Whistler.Domain.Phone.Bank;
using Whistler.Infrastructure.Common;
using Whistler.Domain.Phone.News;
using Whistler.Domain.Fractions;

namespace Whistler.Infrastructure.DataAccess
{
    public class ServerContext : DbContext
    {
        #region Phone

        /// <summary>
        /// Телефоны.
        /// </summary>
        public DbSet<Phone> Phones { get; set; }

        #region App: Contacts
        /// <summary>
        /// Сим-карты.
        /// </summary>
        public DbSet<SimCard> SimCards { get; set; }

        /// <summary>
        /// Контакты.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// История звонок.
        /// </summary>
        public DbSet<Call> CallHistory { get; set; }

        /// <summary>
        /// Список заблокированных контактов.
        /// </summary>
        public DbSet<Block> BlockedContacts { get; set; }
        #endregion

        #region App: Messenger
        /// <summary>
        /// Мессенджер: Аккаунты.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Мессенджер: Отношение аккаунта к чату/каналу.
        /// </summary>
        public DbSet<AccountToChat> AccountsToChats { get; set; }

        /// <summary>
        /// Мессенджер: Личные, групповые чаты и каналы.
        /// </summary>
        public DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// Мессенджер: Сообщения в личных и групповых чатах.
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Мессенджер: Посты в каналах.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Мессенджер: Контакты в мессенджере.
        /// </summary>
        public DbSet<MsgContact> MsgContacts { get; set; }
        #endregion

        #region App: Taxi
        /// <summary>
        /// История заказов в приложении такси.
        /// </summary>
        public DbSet<OrderHistoryItem> TaxiOrdersHistory { get; set; }
        #endregion

        #region App: Bank
        /// <summary>
        /// История банковских переводов
        /// </summary>
        public DbSet<TransactionHistoryItem> BankTransactionHistory { get; set; }
        #endregion

        #region App: News
        /// <summary>
        /// Объявления
        /// </summary>
        public DbSet<Advert> Adverts { get; set; }
        #endregion


        #endregion



        /// <summary>
        /// Доступы к фракциям
        /// </summary>
        public DbSet<Access> Accesses { get; set; }
        #region Fractions


        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection"); //"Server=localhost;uid=root;pwd=root;port=3306;database=gtago-core;";// 
            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Contacts
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.TargetNumber);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.HolderSimCard)
                .WithMany(s => s.Contacts)
                .HasForeignKey(c => c.HolderSimCardId);

            modelBuilder.Entity<Domain.Phone.Phone>()
                .Property(p => p.InstalledAppsIds)
                .HasConversion(
                    appIds => JsonConvert.SerializeObject(appIds),
                    appIds => JsonConvert.DeserializeObject<List<AppId>>(appIds)
                );

            modelBuilder.Entity<SimCard>()
                .HasIndex(s => s.Number)
                .IsUnique();

            modelBuilder.Entity<Block>()
                .HasKey(b => new { b.SimCardId, b.TargetNumber });

            modelBuilder.Entity<Call>()
                .HasIndex(c => c.TargetNumber);
            #endregion

            #region Messenger
            modelBuilder.Entity<AccountToChat>()
                .HasKey(ac => new { ac.AccountId, ac.ChatId });

            modelBuilder.Entity<AccountToChat>()
                .HasOne(ac => ac.Account);

            modelBuilder.Entity<AccountToChat>()
                .HasOne(ac => ac.Chat)
                .WithMany(c => c.AccountToChats)
                .HasForeignKey(ac => ac.ChatId);

            modelBuilder.Entity<AccountToChat>()
                .Property(ac => ac.Permissions)
                .HasConversion(
                    permissions => JsonConvert.SerializeObject(permissions),
                    permissions => JsonConvert.DeserializeObject<List<Permission>>(permissions)
                );

            modelBuilder.Entity<Chat>()
                .HasIndex(c => c.InviteCode)
                .IsUnique();

            modelBuilder.Entity<Message>()
                .Property(m => m.Attachments)
                .HasConversion(
                    attachments => JsonConvert.SerializeObject(attachments, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
                    attachments => JsonConvert.DeserializeObject<IReadOnlyList<Attachment>>(attachments, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                );
            #endregion

            modelBuilder.Entity<Access>()
                .Property(ac => ac.AccessList)
                .HasConversion(
                    access => JsonConvert.SerializeObject(access),
                    access => JsonConvert.DeserializeObject<List<AccessType>>(access)
                );
        }
    }
}
