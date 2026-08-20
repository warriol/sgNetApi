using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escalafones",
                columns: table => new
                {
                    IdEscalafon = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Abreviatura = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalafones", x => x.IdEscalafon);
                });

            migrationBuilder.CreateTable(
                name: "Grados",
                columns: table => new
                {
                    IdGrado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    Texto = table.Column<string>(type: "text", nullable: false),
                    Abreviatura = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grados", x => x.IdGrado);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IdPermiso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesEjecutoras",
                columns: table => new
                {
                    IdUuee = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Siglas = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesEjecutoras", x => x.IdUuee);
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdPermiso = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => new { x.IdRol, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permisos_IdPermiso",
                        column: x => x.IdPermiso,
                        principalTable: "Permisos",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dependencias",
                columns: table => new
                {
                    IdDependencia = table.Column<int>(type: "integer", nullable: false),
                    IdUuee = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Siglas = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencias", x => new { x.IdDependencia, x.IdUuee });
                    table.ForeignKey(
                        name: "FK_Dependencias_UnidadesEjecutoras_IdUuee",
                        column: x => x.IdUuee,
                        principalTable: "UnidadesEjecutoras",
                        principalColumn: "IdUuee",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Ci = table.Column<long>(type: "bigint", nullable: false),
                    NombreUsuario = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Celular = table.Column<long>(type: "bigint", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "integer", nullable: false),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiradoPorInactividad = table.Column<bool>(type: "boolean", nullable: false),
                    Creado = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdGrado = table.Column<int>(type: "integer", nullable: false),
                    GradoIdGrado = table.Column<int>(type: "integer", nullable: false),
                    IdEscalafon = table.Column<int>(type: "integer", nullable: false),
                    EscalafonIdEscalafon = table.Column<int>(type: "integer", nullable: false),
                    IdUuee = table.Column<int>(type: "integer", nullable: false),
                    UnidadEjecutoraIdUuee = table.Column<int>(type: "integer", nullable: false),
                    IdDependencia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Ci);
                    table.ForeignKey(
                        name: "FK_Usuarios_Dependencias_IdDependencia_IdUuee",
                        columns: x => new { x.IdDependencia, x.IdUuee },
                        principalTable: "Dependencias",
                        principalColumns: new[] { "IdDependencia", "IdUuee" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Escalafones_EscalafonIdEscalafon",
                        column: x => x.EscalafonIdEscalafon,
                        principalTable: "Escalafones",
                        principalColumn: "IdEscalafon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Grados_GradoIdGrado",
                        column: x => x.GradoIdGrado,
                        principalTable: "Grados",
                        principalColumn: "IdGrado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_UnidadesEjecutoras_UnidadEjecutoraIdUuee",
                        column: x => x.UnidadEjecutoraIdUuee,
                        principalTable: "UnidadesEjecutoras",
                        principalColumn: "IdUuee",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialesPasswords",
                columns: table => new
                {
                    IdHistorialPassword = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioCi = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialesPasswords", x => x.IdHistorialPassword);
                    table.ForeignKey(
                        name: "FK_HistorialesPasswords_Usuarios_UsuarioCi",
                        column: x => x.UsuarioCi,
                        principalTable: "Usuarios",
                        principalColumn: "Ci",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialesUsuarios",
                columns: table => new
                {
                    IdHistorial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoAccion = table.Column<string>(type: "text", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    RealizadoPor = table.Column<string>(type: "text", nullable: false),
                    UsuarioCi = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialesUsuarios", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HistorialesUsuarios_Usuarios_UsuarioCi",
                        column: x => x.UsuarioCi,
                        principalTable: "Usuarios",
                        principalColumn: "Ci",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPermiso",
                columns: table => new
                {
                    UsuarioCi = table.Column<long>(type: "bigint", nullable: false),
                    IdPermiso = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPermiso", x => new { x.UsuarioCi, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK_UsuarioPermiso_Permisos_IdPermiso",
                        column: x => x.IdPermiso,
                        principalTable: "Permisos",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPermiso_Usuarios_UsuarioCi",
                        column: x => x.UsuarioCi,
                        principalTable: "Usuarios",
                        principalColumn: "Ci",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    UsuarioCi = table.Column<long>(type: "bigint", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.UsuarioCi, x.IdRol });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuarios_UsuarioCi",
                        column: x => x.UsuarioCi,
                        principalTable: "Usuarios",
                        principalColumn: "Ci",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dependencias_IdUuee",
                table: "Dependencias",
                column: "IdUuee");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesPasswords_UsuarioCi",
                table: "HistorialesPasswords",
                column: "UsuarioCi");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesUsuarios_UsuarioCi",
                table: "HistorialesUsuarios",
                column: "UsuarioCi");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_IdPermiso",
                table: "RolPermiso",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermiso_IdPermiso",
                table: "UsuarioPermiso",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_IdRol",
                table: "UsuarioRol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EscalafonIdEscalafon",
                table: "Usuarios",
                column: "EscalafonIdEscalafon");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_GradoIdGrado",
                table: "Usuarios",
                column: "GradoIdGrado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdDependencia_IdUuee",
                table: "Usuarios",
                columns: new[] { "IdDependencia", "IdUuee" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UnidadEjecutoraIdUuee",
                table: "Usuarios",
                column: "UnidadEjecutoraIdUuee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialesPasswords");

            migrationBuilder.DropTable(
                name: "HistorialesUsuarios");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "UsuarioPermiso");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Dependencias");

            migrationBuilder.DropTable(
                name: "Escalafones");

            migrationBuilder.DropTable(
                name: "Grados");

            migrationBuilder.DropTable(
                name: "UnidadesEjecutoras");
        }
    }
}
