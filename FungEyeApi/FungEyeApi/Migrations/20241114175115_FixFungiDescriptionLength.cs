using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FungEyeApi.Migrations
{
    /// <inheritdoc />
    public partial class FixFungiDescriptionLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Fungies",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Fungies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: -1,
                oldNullable: true);
        }
    }
}
