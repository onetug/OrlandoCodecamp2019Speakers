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
    [Migration("20190224000000001_Add_Event_Store")]
    public partial class AddEventStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.Sql(@"
CREATE TABLE [dbo].[EventStore](
    [CommitId] [uniqueidentifier] NOT NULL,
    [AggregateId] [uniqueidentifier] NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [Version] [bigint] NOT NULL,
	[EventType] [nvarchar](300) NOT NULL,
    [EventData] [nvarchar](max) NOT NULL,
	[Offset] [bigint] IDENTITY(1,1)
    PRIMARY KEY CLUSTERED ([AggregateId], [Version])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
        }
    }
}
