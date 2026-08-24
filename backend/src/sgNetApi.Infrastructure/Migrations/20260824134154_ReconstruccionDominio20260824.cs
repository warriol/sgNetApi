using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReconstruccionDominio20260824 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialesPasswords_Usuarios_UsuarioCi",
                table: "HistorialesPasswords");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialesUsuarios_Usuarios_UsuarioCi",
                table: "HistorialesUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPermiso_Usuarios_UsuarioCi",
                table: "UsuarioPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Usuarios_UsuarioCi",
                table: "UsuarioRol");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia_IdUuee",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Escalafones_IdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Grados_IdGrado",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_UnidadesEjecutoras_IdUuee",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdDependencia_IdUuee",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRol",
                table: "UsuarioRol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioPermiso",
                table: "UsuarioPermiso");

            migrationBuilder.DropIndex(
                name: "IX_HistorialesUsuarios_UsuarioCi",
                table: "HistorialesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_HistorialesPasswords_UsuarioCi",
                table: "HistorialesPasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dependencias",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioCi",
                table: "UsuarioRol");

            migrationBuilder.DropColumn(
                name: "UsuarioCi",
                table: "UsuarioPermiso");

            migrationBuilder.DropColumn(
                name: "UsuarioCi",
                table: "HistorialesUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioCi",
                table: "HistorialesPasswords");

            migrationBuilder.DropColumn(
                name: "UsuarioCi",
                table: "AuditoriaLogs");

            migrationBuilder.RenameColumn(
                name: "IdUuee",
                table: "Usuarios",
                newName: "IdNacionalidad");

            migrationBuilder.RenameColumn(
                name: "Creado",
                table: "Usuarios",
                newName: "FechaCreacion");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_IdUuee",
                table: "Usuarios",
                newName: "IX_Usuarios_IdNacionalidad");

            migrationBuilder.RenameColumn(
                name: "TiempoEjecucionMs",
                table: "AuditoriaLogs",
                newName: "DuracionMs");

            migrationBuilder.RenameColumn(
                name: "Excepcion",
                table: "AuditoriaLogs",
                newName: "UsuarioNombreUsuario");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AuditoriaLogs",
                newName: "IdLog");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<string>(
                name: "NombreUsuario",
                table: "Usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "IdGrado",
                table: "Usuarios",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdEscalafon",
                table: "Usuarios",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IdDependencia",
                table: "Usuarios",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Celular",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "Ci",
                table: "Usuarios",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaNacimiento",
                table: "Usuarios",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "IdEstadoCivil",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdProfesion",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "UsuarioRol",
                type: "character varying(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "UsuarioPermiso",
                type: "character varying(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombreUsuario",
                table: "HistorialesUsuarios",
                type: "character varying(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "HistorialesPasswords",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombreUsuario",
                table: "HistorialesPasswords",
                type: "character varying(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "IdDependencia",
                table: "Dependencias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Ruta",
                table: "AuditoriaLogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MetodoHttp",
                table: "AuditoriaLogs",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IpOrigen",
                table: "AuditoriaLogs",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "PayloadRequest",
                table: "AuditoriaLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "NombreUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRol",
                table: "UsuarioRol",
                columns: new[] { "NombreUsuario", "IdRol" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioPermiso",
                table: "UsuarioPermiso",
                columns: new[] { "NombreUsuario", "IdPermiso" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dependencias",
                table: "Dependencias",
                column: "IdDependencia");

            migrationBuilder.CreateTable(
                name: "EstadosCiviles",
                columns: table => new
                {
                    IdEstadoCivil = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCiviles", x => x.IdEstadoCivil);
                });

            migrationBuilder.CreateTable(
                name: "Nacionalidades",
                columns: table => new
                {
                    IdNacionalidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    CodigoIso = table.Column<string>(type: "text", nullable: false),
                    EsUruguaya = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nacionalidades", x => x.IdNacionalidad);
                });

            migrationBuilder.CreateTable(
                name: "Profesiones",
                columns: table => new
                {
                    IdProfesion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesiones", x => x.IdProfesion);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Ci",
                table: "Usuarios",
                column: "Ci",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdDependencia",
                table: "Usuarios",
                column: "IdDependencia");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdEstadoCivil",
                table: "Usuarios",
                column: "IdEstadoCivil");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdProfesion",
                table: "Usuarios",
                column: "IdProfesion");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesUsuarios_UsuarioNombreUsuario",
                table: "HistorialesUsuarios",
                column: "UsuarioNombreUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesPasswords_UsuarioNombreUsuario",
                table: "HistorialesPasswords",
                column: "UsuarioNombreUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCiviles_Nombre",
                table: "EstadosCiviles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nacionalidades_CodigoIso",
                table: "Nacionalidades",
                column: "CodigoIso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nacionalidades_Nombre",
                table: "Nacionalidades",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profesiones_Nombre",
                table: "Profesiones",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialesPasswords_Usuarios_UsuarioNombreUsuario",
                table: "HistorialesPasswords",
                column: "UsuarioNombreUsuario",
                principalTable: "Usuarios",
                principalColumn: "NombreUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialesUsuarios_Usuarios_UsuarioNombreUsuario",
                table: "HistorialesUsuarios",
                column: "UsuarioNombreUsuario",
                principalTable: "Usuarios",
                principalColumn: "NombreUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPermiso_Usuarios_NombreUsuario",
                table: "UsuarioPermiso",
                column: "NombreUsuario",
                principalTable: "Usuarios",
                principalColumn: "NombreUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Usuarios_NombreUsuario",
                table: "UsuarioRol",
                column: "NombreUsuario",
                principalTable: "Usuarios",
                principalColumn: "NombreUsuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia",
                table: "Usuarios",
                column: "IdDependencia",
                principalTable: "Dependencias",
                principalColumn: "IdDependencia",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Escalafones_IdEscalafon",
                table: "Usuarios",
                column: "IdEscalafon",
                principalTable: "Escalafones",
                principalColumn: "IdEscalafon");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_EstadosCiviles_IdEstadoCivil",
                table: "Usuarios",
                column: "IdEstadoCivil",
                principalTable: "EstadosCiviles",
                principalColumn: "IdEstadoCivil");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Grados_IdGrado",
                table: "Usuarios",
                column: "IdGrado",
                principalTable: "Grados",
                principalColumn: "IdGrado");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Nacionalidades_IdNacionalidad",
                table: "Usuarios",
                column: "IdNacionalidad",
                principalTable: "Nacionalidades",
                principalColumn: "IdNacionalidad",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Profesiones_IdProfesion",
                table: "Usuarios",
                column: "IdProfesion",
                principalTable: "Profesiones",
                principalColumn: "IdProfesion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialesPasswords_Usuarios_UsuarioNombreUsuario",
                table: "HistorialesPasswords");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialesUsuarios_Usuarios_UsuarioNombreUsuario",
                table: "HistorialesUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPermiso_Usuarios_NombreUsuario",
                table: "UsuarioPermiso");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRol_Usuarios_NombreUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Escalafones_IdEscalafon",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_EstadosCiviles_IdEstadoCivil",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Grados_IdGrado",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Nacionalidades_IdNacionalidad",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Profesiones_IdProfesion",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "EstadosCiviles");

            migrationBuilder.DropTable(
                name: "Nacionalidades");

            migrationBuilder.DropTable(
                name: "Profesiones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Ci",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdDependencia",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdEstadoCivil",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdProfesion",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRol",
                table: "UsuarioRol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioPermiso",
                table: "UsuarioPermiso");

            migrationBuilder.DropIndex(
                name: "IX_HistorialesUsuarios_UsuarioNombreUsuario",
                table: "HistorialesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_HistorialesPasswords_UsuarioNombreUsuario",
                table: "HistorialesPasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dependencias",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdEstadoCivil",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdProfesion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "UsuarioRol");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "UsuarioPermiso");

            migrationBuilder.DropColumn(
                name: "UsuarioNombreUsuario",
                table: "HistorialesUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioNombreUsuario",
                table: "HistorialesPasswords");

            migrationBuilder.DropColumn(
                name: "PayloadRequest",
                table: "AuditoriaLogs");

            migrationBuilder.RenameColumn(
                name: "IdNacionalidad",
                table: "Usuarios",
                newName: "IdUuee");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Usuarios",
                newName: "Creado");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_IdNacionalidad",
                table: "Usuarios",
                newName: "IX_Usuarios_IdUuee");

            migrationBuilder.RenameColumn(
                name: "UsuarioNombreUsuario",
                table: "AuditoriaLogs",
                newName: "Excepcion");

            migrationBuilder.RenameColumn(
                name: "DuracionMs",
                table: "AuditoriaLogs",
                newName: "TiempoEjecucionMs");

            migrationBuilder.RenameColumn(
                name: "IdLog",
                table: "AuditoriaLogs",
                newName: "Id");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "IdGrado",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEscalafon",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdDependencia",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<long>(
                name: "Ci",
                table: "Usuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Celular",
                table: "Usuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "NombreUsuario",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Usuarios",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioCi",
                table: "UsuarioRol",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioCi",
                table: "UsuarioPermiso",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioCi",
                table: "HistorialesUsuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "HistorialesPasswords",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioCi",
                table: "HistorialesPasswords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "IdDependencia",
                table: "Dependencias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Ruta",
                table: "AuditoriaLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "MetodoHttp",
                table: "AuditoriaLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "IpOrigen",
                table: "AuditoriaLogs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCi",
                table: "AuditoriaLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRol",
                table: "UsuarioRol",
                columns: new[] { "UsuarioCi", "IdRol" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioPermiso",
                table: "UsuarioPermiso",
                columns: new[] { "UsuarioCi", "IdPermiso" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dependencias",
                table: "Dependencias",
                columns: new[] { "IdDependencia", "IdUuee" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdDependencia_IdUuee",
                table: "Usuarios",
                columns: new[] { "IdDependencia", "IdUuee" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesUsuarios_UsuarioCi",
                table: "HistorialesUsuarios",
                column: "UsuarioCi");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesPasswords_UsuarioCi",
                table: "HistorialesPasswords",
                column: "UsuarioCi");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialesPasswords_Usuarios_UsuarioCi",
                table: "HistorialesPasswords",
                column: "UsuarioCi",
                principalTable: "Usuarios",
                principalColumn: "Ci",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialesUsuarios_Usuarios_UsuarioCi",
                table: "HistorialesUsuarios",
                column: "UsuarioCi",
                principalTable: "Usuarios",
                principalColumn: "Ci",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPermiso_Usuarios_UsuarioCi",
                table: "UsuarioPermiso",
                column: "UsuarioCi",
                principalTable: "Usuarios",
                principalColumn: "Ci",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRol_Usuarios_UsuarioCi",
                table: "UsuarioRol",
                column: "UsuarioCi",
                principalTable: "Usuarios",
                principalColumn: "Ci",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia_IdUuee",
                table: "Usuarios",
                columns: new[] { "IdDependencia", "IdUuee" },
                principalTable: "Dependencias",
                principalColumns: new[] { "IdDependencia", "IdUuee" },
                onDelete: ReferentialAction.Cascade);

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
    }
}
