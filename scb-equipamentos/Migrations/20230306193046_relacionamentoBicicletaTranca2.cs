using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace scb_equipamentos.Migrations
{
    /// <inheritdoc />
    public partial class relacionamentoBicicletaTranca2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicicletas_Trancas_Id",
                table: "Bicicletas");

            migrationBuilder.AddColumn<int>(
                name: "BicicletaId",
                table: "Trancas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicicletas_Trancas_TrancaId",
                table: "Bicicletas");

            migrationBuilder.DropIndex(
                name: "IX_Bicicletas_TrancaId",
                table: "Bicicletas");

            migrationBuilder.DropColumn(
                name: "BicicletaId",
                table: "Trancas");

            migrationBuilder.DropColumn(
                name: "TrancaId",
                table: "Bicicletas");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Bicicletas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Bicicletas_Trancas_Id",
                table: "Bicicletas",
                column: "Id",
                principalTable: "Trancas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
