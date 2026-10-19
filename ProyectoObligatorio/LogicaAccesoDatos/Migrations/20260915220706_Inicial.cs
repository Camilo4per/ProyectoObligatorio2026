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
                    idHistoria = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
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
                    idCapituloInicial = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Titulo_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinopsis_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historias_Capitulos_idCapituloInicial",
                        column: x => x.idCapituloInicial,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Opcion",
                columns: table => new
                {
                    idCapitulo = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCapDestino = table.Column<int>(type: "int", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opcion", x => new { x.idCapitulo, x.Id });
                    table.ForeignKey(
                        name: "FK_Opcion_Capitulos_idCapDestino",
                        column: x => x.idCapDestino,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Opcion_Capitulos_idCapitulo",
                        column: x => x.idCapitulo,
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
                    idUsuario = table.Column<int>(type: "int", nullable: true),
                    idHistoria = table.Column<int>(type: "int", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    idFinal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturas_Capitulos_idFinal",
                        column: x => x.idFinal,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Historias_idHistoria",
                        column: x => x.idHistoria,
                        principalTable: "Historias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capitulos_idHistoria",
                table: "Capitulos",
                column: "idHistoria");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_HistoriaId",
                table: "Categorias",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_idCapituloInicial",
                table: "Historias",
                column: "idCapituloInicial");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_idFinal",
                table: "Lecturas",
                column: "idFinal");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_idHistoria",
                table: "Lecturas",
                column: "idHistoria");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_idUsuario",
                table: "Lecturas",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Opcion_idCapDestino",
                table: "Opcion",
                column: "idCapDestino");

            migrationBuilder.AddForeignKey(
                name: "FK_Capitulos_Historias_idHistoria",
                table: "Capitulos",
                column: "idHistoria",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capitulos_Historias_idHistoria",
                table: "Capitulos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Lecturas");

            migrationBuilder.DropTable(
                name: "Opcion");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "Capitulos");
        }
    }
}
