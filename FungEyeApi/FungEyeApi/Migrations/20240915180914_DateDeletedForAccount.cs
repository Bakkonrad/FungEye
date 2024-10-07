using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FungEyeApi.Migrations
{
    /// <inheritdoc />
    public partial class DateDeletedForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Users");
        }
    }
}
