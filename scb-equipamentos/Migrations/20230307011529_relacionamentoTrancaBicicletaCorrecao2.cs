using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class relacionamentoTrancaBicicletaCorrecao2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas");

            migrationBuilder.AlterColumn<int>(
                name: "BicicletaId",
                table: "Trancas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas",
                column: "BicicletaId",
                principalTable: "Bicicletas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas");

            migrationBuilder.AlterColumn<int>(
                name: "BicicletaId",
                table: "Trancas",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaId",
                table: "Trancas",
                column: "BicicletaId",
                principalTable: "Bicicletas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
