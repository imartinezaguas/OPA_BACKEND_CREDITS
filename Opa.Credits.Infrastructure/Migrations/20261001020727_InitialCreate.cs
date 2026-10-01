using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Opa.Credits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asociados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identificacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asociados", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "creditos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numerocredito = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    asociadoid = table.Column<int>(type: "integer", nullable: false),
                    tipocredito = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    valorsolicitado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tasainteres = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    numerocuotas = table.Column<int>(type: "integer", nullable: false),
                    formapago = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fechasolicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaactualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_creditos", x => x.id);
                    table.ForeignKey(
                        name: "fk_creditos_asociados_asociadoid",
                        column: x => x.asociadoid,
                        principalTable: "asociados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historialcreditos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creditoid = table.Column<int>(type: "integer", nullable: false),
                    estadoanterior = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    estadonuevo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fechacambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historialcreditos", x => x.id);
                    table.ForeignKey(
                        name: "fk_historialcreditos_creditos_creditoid",
                        column: x => x.creditoid,
                        principalTable: "creditos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asociados_identificacion",
                table: "asociados",
                column: "identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_creditos_asociadoid",
                table: "creditos",
                column: "asociadoid");

            migrationBuilder.CreateIndex(
                name: "ix_creditos_numerocredito",
                table: "creditos",
                column: "numerocredito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_historialcreditos_creditoid",
                table: "historialcreditos",
                column: "creditoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historialcreditos");

            migrationBuilder.DropTable(
                name: "creditos");

            migrationBuilder.DropTable(
                name: "asociados");
        }
    }
}
