using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTurnoAsignado",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoTurno",
                table: "Turnos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdTurnoAsignado",
                table: "Usuarios",
                column: "IdTurnoAsignado");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Turnos_IdTurnoAsignado",
                table: "Usuarios",
                column: "IdTurnoAsignado",
                principalTable: "Turnos",
                principalColumn: "IdTurno",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Turnos_IdTurnoAsignado",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdTurnoAsignado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdTurnoAsignado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TipoTurno",
                table: "Turnos");
        }
    }
}
