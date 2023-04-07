using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoIdBicicleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaNumero",
                table: "Trancas");

            migrationBuilder.DropIndex(
                name: "IX_Trancas_BicicletaNumero",
                table: "Trancas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bicicletas",
                table: "Bicicletas");

            migrationBuilder.DropColumn(
                name: "BicicletaNumero",
                table: "Trancas");

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bicicletas",
                table: "Bicicletas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bicicletas_Trancas_Id",
                table: "Bicicletas",
                column: "Id",
                principalTable: "Trancas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicicletas_Trancas_Id",
                table: "Bicicletas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bicicletas",
                table: "Bicicletas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Bicicletas");

            migrationBuilder.AddColumn<int>(
                name: "BicicletaNumero",
                table: "Trancas",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bicicletas",
                table: "Bicicletas",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_Trancas_BicicletaNumero",
                table: "Trancas",
                column: "BicicletaNumero");

            migrationBuilder.AddForeignKey(
                name: "FK_Trancas_Bicicletas_BicicletaNumero",
                table: "Trancas",
                column: "BicicletaNumero",
                principalTable: "Bicicletas",
                principalColumn: "Numero");
        }
    }
}
