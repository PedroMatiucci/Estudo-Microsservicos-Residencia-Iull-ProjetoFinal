using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class CriandoTabelaTranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trancas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BicicletaNumero = table.Column<int>(type: "integer", nullable: true),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    AnoFabricacao = table.Column<int>(type: "integer", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trancas_Bicicletas_BicicletaNumero",
                        column: x => x.BicicletaNumero,
                        principalTable: "Bicicletas",
                        principalColumn: "Numero");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trancas_BicicletaNumero",
                table: "Trancas",
                column: "BicicletaNumero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trancas");
        }
    }
}
