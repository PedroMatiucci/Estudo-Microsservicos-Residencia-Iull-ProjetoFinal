using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class CriandoTabelaTotems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotemId",
                table: "Trancas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Totems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Localizacao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trancas_TotemId",
                table: "Trancas",
                column: "TotemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trancas_Totems_TotemId",
                table: "Trancas",
                column: "TotemId",
                principalTable: "Totems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trancas_Totems_TotemId",
                table: "Trancas");

            migrationBuilder.DropTable(
                name: "Totems");

            migrationBuilder.DropIndex(
                name: "IX_Trancas_TotemId",
                table: "Trancas");

            migrationBuilder.DropColumn(
                name: "TotemId",
                table: "Trancas");
        }
    }
}
