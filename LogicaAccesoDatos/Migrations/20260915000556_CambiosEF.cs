using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_Capitulos_CapituloInicialId",
                table: "Historias");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Historias_HistoriaId",
                table: "Lecturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Usuarios_UsuarioId",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Lecturas_HistoriaId",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Lecturas_UsuarioId",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Historias_CapituloInicialId",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "HistoriaId",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "CapituloInicialId",
                table: "Historias");

            migrationBuilder.AddColumn<int>(
                name: "idFinal",
                table: "Lecturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idHistoria",
                table: "Lecturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idUsuario",
                table: "Lecturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idCapituloInicial",
                table: "Historias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idHistoria",
                table: "Capitulos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_idHistoria",
                table: "Lecturas",
                column: "idHistoria");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_idUsuario",
                table: "Lecturas",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_idCapituloInicial",
                table: "Historias",
                column: "idCapituloInicial");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_Capitulos_idCapituloInicial",
                table: "Historias",
                column: "idCapituloInicial",
                principalTable: "Capitulos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Historias_idHistoria",
                table: "Lecturas",
                column: "idHistoria",
                principalTable: "Historias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Usuarios_idUsuario",
                table: "Lecturas",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_Capitulos_idCapituloInicial",
                table: "Historias");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Historias_idHistoria",
                table: "Lecturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Usuarios_idUsuario",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Lecturas_idHistoria",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Lecturas_idUsuario",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Historias_idCapituloInicial",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "idFinal",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "idHistoria",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "idUsuario",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "idCapituloInicial",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "idHistoria",
                table: "Capitulos");

            migrationBuilder.AddColumn<int>(
                name: "HistoriaId",
                table: "Lecturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Lecturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CapituloInicialId",
                table: "Historias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_HistoriaId",
                table: "Lecturas",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_UsuarioId",
                table: "Lecturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_CapituloInicialId",
                table: "Historias",
                column: "CapituloInicialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_Capitulos_CapituloInicialId",
                table: "Historias",
                column: "CapituloInicialId",
                principalTable: "Capitulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Historias_HistoriaId",
                table: "Lecturas",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Usuarios_UsuarioId",
                table: "Lecturas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
