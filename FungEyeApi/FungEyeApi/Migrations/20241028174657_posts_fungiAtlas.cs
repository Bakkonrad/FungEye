using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FungEyeApi.Migrations
{
    /// <inheritdoc />
    public partial class posts_fungiAtlas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fungies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LatinName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PolishName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Edibility = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Toxicity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Habitat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fungies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FungiImageEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FungiEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FungiImageEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FungiImageEntity_Fungies_FungiEntityId",
                        column: x => x.FungiEntityId,
                        principalTable: "Fungies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFungiCollections",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FungiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFungiCollections", x => new { x.UserId, x.FungiId });
                    table.ForeignKey(
                        name: "FK_UserFungiCollections_Fungies_FungiId",
                        column: x => x.FungiId,
                        principalTable: "Fungies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFungiCollections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FungiImageEntity_FungiEntityId",
                table: "FungiImageEntity",
                column: "FungiEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFungiCollections_FungiId",
                table: "UserFungiCollections",
                column: "FungiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FungiImageEntity");

            migrationBuilder.DropTable(
                name: "UserFungiCollections");

            migrationBuilder.DropTable(
                name: "Fungies");
        }
    }
}
