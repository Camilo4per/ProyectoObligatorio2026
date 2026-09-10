using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    NombreCompleto_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto_Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email_Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Capitulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Texto_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: true),
                    CantLlegaron_Valor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CapituloInicialId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Titulo_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinopsis_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historias_Capitulos_CapituloInicialId",
                        column: x => x.CapituloInicialId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lecturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalAlcanzadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                        column: x => x.FinalAlcanzadoId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capitulos_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_HistoriaId",
                table: "Categorias",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_CapituloInicialId",
                table: "Historias",
                column: "CapituloInicialId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_FinalAlcanzadoId",
                table: "Lecturas",
                column: "FinalAlcanzadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_HistoriaId",
                table: "Lecturas",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_UsuarioId",
                table: "Lecturas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Capitulos_Historias_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capitulos_Historias_HistoriaId",
                table: "Capitulos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Lecturas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "Capitulos");
        }
    }
}
