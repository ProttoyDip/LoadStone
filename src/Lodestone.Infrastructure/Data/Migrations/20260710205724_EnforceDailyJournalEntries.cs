using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodestone.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnforceDailyJournalEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MoodJournalEntries_StudentProfileId",
                table: "MoodJournalEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDayUtc",
                table: "MoodJournalEntries",
                type: "date",
                nullable: false,
                computedColumnSql: "CONVERT(date, [EntryDateUtc])",
                stored: true);

            // Preserve the first historical check-in per student/day and soft-delete only
            // later duplicates before creating the new active-entry uniqueness constraint.
            migrationBuilder.Sql(
                """
                ;WITH RankedEntries AS
                (
                    SELECT [Id],
                           ROW_NUMBER() OVER
                           (
                               PARTITION BY [StudentProfileId], [EntryDayUtc]
                               ORDER BY [EntryDateUtc], [Id]
                           ) AS [RowNumber]
                    FROM [MoodJournalEntries]
                    WHERE [IsDeleted] = 0
                )
                UPDATE [Entry]
                SET [IsDeleted] = 1,
                    [DeletedAtUtc] = SYSUTCDATETIME(),
                    [DeletedBy] = N'System:DailyJournalEntryLimit'
                FROM [MoodJournalEntries] AS [Entry]
                INNER JOIN RankedEntries AS [Ranked]
                    ON [Entry].[Id] = [Ranked].[Id]
                WHERE [Ranked].[RowNumber] > 1;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_MoodJournalEntries_StudentProfileId_EntryDateUtc",
                table: "MoodJournalEntries",
                columns: new[] { "StudentProfileId", "EntryDateUtc" });

            migrationBuilder.CreateIndex(
                name: "UX_MoodJournalEntries_StudentProfileId_EntryDayUtc_Active",
                table: "MoodJournalEntries",
                columns: new[] { "StudentProfileId", "EntryDayUtc" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MoodJournalEntries_StudentProfileId_EntryDateUtc",
                table: "MoodJournalEntries");

            migrationBuilder.DropIndex(
                name: "UX_MoodJournalEntries_StudentProfileId_EntryDayUtc_Active",
                table: "MoodJournalEntries");

            migrationBuilder.DropColumn(
                name: "EntryDayUtc",
                table: "MoodJournalEntries");

            migrationBuilder.CreateIndex(
                name: "IX_MoodJournalEntries_StudentProfileId",
                table: "MoodJournalEntries",
                column: "StudentProfileId");
        }
    }
}
