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
    [Migration("20190218163427_RemoveTransactionsView")]
    public partial class RemoveTransactionsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.Sql("DROP VIEW [TransactionsView]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
        }
    }
}
