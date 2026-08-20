using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicialCorregida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Escalafones_EscalafonIdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Grados_GradoIdGrado",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_UnidadesEjecutoras_UnidadEjecutoraIdUuee",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_EscalafonIdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_GradoIdGrado",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_UnidadEjecutoraIdUuee",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EscalafonIdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "GradoIdGrado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UnidadEjecutoraIdUuee",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdEscalafon",
                table: "Usuarios",
                column: "IdEscalafon");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdGrado",
                table: "Usuarios",
                column: "IdGrado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdUuee",
                table: "Usuarios",
                column: "IdUuee");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Escalafones_IdEscalafon",
                table: "Usuarios",
                column: "IdEscalafon",
                principalTable: "Escalafones",
                principalColumn: "IdEscalafon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Grados_IdGrado",
                table: "Usuarios",
                column: "IdGrado",
                principalTable: "Grados",
                principalColumn: "IdGrado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_UnidadesEjecutoras_IdUuee",
                table: "Usuarios",
                column: "IdUuee",
                principalTable: "UnidadesEjecutoras",
                principalColumn: "IdUuee",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Escalafones_IdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Grados_IdGrado",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_UnidadesEjecutoras_IdUuee",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdGrado",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdUuee",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "EscalafonIdEscalafon",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GradoIdGrado",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnidadEjecutoraIdUuee",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EscalafonIdEscalafon",
                table: "Usuarios",
                column: "EscalafonIdEscalafon");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_GradoIdGrado",
                table: "Usuarios",
                column: "GradoIdGrado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UnidadEjecutoraIdUuee",
                table: "Usuarios",
                column: "UnidadEjecutoraIdUuee");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Escalafones_EscalafonIdEscalafon",
                table: "Usuarios",
                column: "EscalafonIdEscalafon",
                principalTable: "Escalafones",
                principalColumn: "IdEscalafon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Grados_GradoIdGrado",
                table: "Usuarios",
                column: "GradoIdGrado",
                principalTable: "Grados",
                principalColumn: "IdGrado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_UnidadesEjecutoras_UnidadEjecutoraIdUuee",
                table: "Usuarios",
                column: "UnidadEjecutoraIdUuee",
                principalTable: "UnidadesEjecutoras",
                principalColumn: "IdUuee",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
