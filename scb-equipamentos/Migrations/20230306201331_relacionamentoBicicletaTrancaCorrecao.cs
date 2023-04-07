using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class relacionamentoBicicletaTrancaCorrecao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicicletas_Trancas_TrancaId",
                table: "Bicicletas");

            migrationBuilder.DropIndex(
                name: "IX_Bicicletas_TrancaId",
                table: "Bicicletas");

            migrationBuilder.DropColumn(
                name: "TrancaId",
                table: "Bicicletas");

            migrationBuilder.CreateIndex(
                name: "IX_Trancas_BicicletaId",
                table: "Trancas",
                column: "BicicletaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas",
                column: "BicicletaId",
                principalTable: "Bicicletas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas");

            migrationBuilder.DropIndex(
                name: "IX_Trancas_BicicletaId",
                table: "Trancas");

            migrationBuilder.AddColumn<int>(
                name: "TrancaId",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bicicletas_TrancaId",
                table: "Bicicletas",
                column: "TrancaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bicicletas_Trancas_TrancaId",
                table: "Bicicletas",
                column: "TrancaId",
                principalTable: "Trancas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
