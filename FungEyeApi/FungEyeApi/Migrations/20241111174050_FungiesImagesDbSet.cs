using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FungEyeApi.Migrations
{
    /// <inheritdoc />
    public partial class FungiesImagesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FungiImageEntity_Fungies_FungiEntityId",
                table: "FungiImageEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FungiImageEntity",
                table: "FungiImageEntity");

            migrationBuilder.RenameTable(
                name: "FungiImageEntity",
                newName: "FungiesImages");

            migrationBuilder.RenameIndex(
                name: "IX_FungiImageEntity_FungiEntityId",
                table: "FungiesImages",
                newName: "IX_FungiesImages_FungiEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FungiesImages",
                table: "FungiesImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FungiesImages_Fungies_FungiEntityId",
                table: "FungiesImages",
                column: "FungiEntityId",
                principalTable: "Fungies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FungiesImages_Fungies_FungiEntityId",
                table: "FungiesImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FungiesImages",
                table: "FungiesImages");

            migrationBuilder.RenameTable(
                name: "FungiesImages",
                newName: "FungiImageEntity");

            migrationBuilder.RenameIndex(
                name: "IX_FungiesImages_FungiEntityId",
                table: "FungiImageEntity",
                newName: "IX_FungiImageEntity_FungiEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FungiImageEntity",
                table: "FungiImageEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FungiImageEntity_Fungies_FungiEntityId",
                table: "FungiImageEntity",
                column: "FungiEntityId",
                principalTable: "Fungies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
