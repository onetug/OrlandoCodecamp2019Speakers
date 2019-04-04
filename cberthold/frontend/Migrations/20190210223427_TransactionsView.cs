using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using frontend.Data;

namespace frontend.Migrations
{
    [DbContext(typeof(BankAccountsContext))]
    [Migration("20190210223427_TransactionsView")]
    public partial class TransactionsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"
CREATE VIEW [TransactionsView]
AS
    SELECT
        AccountId = d.AccountId
        , TransactionId = d.DepositId
        , Type = 'Deposit'
        , Date = d.Date
        , Amount = d.Amount
    FROM [Deposits] d
    UNION ALL
    SELECT
        AccountId = w.AccountId
        , TransactionId = w.WithdrawalId
        , Type = 'Withdrawal'
        , Date = w.Date
        , Amount = w.Amount * -1.00
    FROM [Withdrawals] w

GO");

            migrationBuilder.Sql("INSERT INTO Accounts (AccountId, AccountName, CurrentBalance) VALUES ('4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea','Chris Berthold', 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [TransactionsView]");
        }
    }
}
