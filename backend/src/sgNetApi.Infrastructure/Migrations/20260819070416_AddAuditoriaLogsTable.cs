using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoriaLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriaLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioCi = table.Column<string>(type: "text", nullable: false),
                    IpOrigen = table.Column<string>(type: "text", nullable: false),
                    MetodoHttp = table.Column<string>(type: "text", nullable: false),
                    Ruta = table.Column<string>(type: "text", nullable: false),
                    CodigoEstado = table.Column<int>(type: "integer", nullable: false),
                    TiempoEjecucionMs = table.Column<long>(type: "bigint", nullable: false),
                    Excepcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLogs");
        }
    }
}
