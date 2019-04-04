using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace frontend.Data
{
    public interface IDbContext
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        IModel Model { get; }
        
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;
    }

    public interface IBankAccountsContext : IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Deposit> Deposits { get; set; }
        DbSet<Withdrawal> Withdrawals { get; set; }
        DbSet<TransactionsView> Transactions { get; set; }
    }

    public class BankAccountsContext : DbContext, IBankAccountsContext
    {
        public BankAccountsContext(DbContextOptions<BankAccountsContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }

        public DbSet<TransactionsView> Transactions { get; set; }

    }

    public class TransactionsView
    {
        public Guid AccountId { get; set; }
        [Key]
        public Guid TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Account
    {

        public Account() {
            AccountId = Guid.NewGuid();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal CurrentBalance { get; set; }

        public List<Deposit> Deposits { get; set; }
        public List<Withdrawal> Withdrawals { get; set; }
    }

    public class Deposit
    {
        public Deposit() {
            DepositId = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DepositId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }

    public class Withdrawal
    {
        public Withdrawal() {
            WithdrawalId = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WithdrawalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}