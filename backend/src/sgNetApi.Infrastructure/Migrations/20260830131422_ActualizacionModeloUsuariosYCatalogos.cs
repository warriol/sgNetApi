using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace sgNetApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionModeloUsuariosYCatalogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dependencias_UnidadesEjecutoras_IdUuee",
                table: "Dependencias");

            migrationBuilder.AlterColumn<string>(
                name: "Siglas",
                table: "Dependencias",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Dependencias",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "IdDireccion",
                table: "Dependencias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTurno",
                table: "Dependencias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuarioJefe",
                table: "Dependencias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuarioSegundoJefe",
                table: "Dependencias",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArmasEstados",
                columns: table => new
                {
                    IdArmaEstado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmasEstados", x => x.IdArmaEstado);
                });

            migrationBuilder.CreateTable(
                name: "ArmasMarcas",
                columns: table => new
                {
                    IdArmaMarca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmasMarcas", x => x.IdArmaMarca);
                });

            migrationBuilder.CreateTable(
                name: "ArmasTipos",
                columns: table => new
                {
                    IdArmaTipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmasTipos", x => x.IdArmaTipo);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosAntibalaPoliciales",
                columns: table => new
                {
                    IdChalecoAntibalaPolicial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdChalecoTipo = table.Column<int>(type: "integer", nullable: false),
                    IdChalecoMarca = table.Column<int>(type: "integer", nullable: false),
                    IdChalecoModelo = table.Column<int>(type: "integer", nullable: false),
                    IdChalecoTalle = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdChalecoEstado = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosAntibalaPoliciales", x => x.IdChalecoAntibalaPolicial);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosEstados",
                columns: table => new
                {
                    IdChalecoEstado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosEstados", x => x.IdChalecoEstado);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosMarcas",
                columns: table => new
                {
                    IdChalecoMarca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosMarcas", x => x.IdChalecoMarca);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosModelos",
                columns: table => new
                {
                    IdChalecoModelo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosModelos", x => x.IdChalecoModelo);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosTalles",
                columns: table => new
                {
                    IdChalecoTalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosTalles", x => x.IdChalecoTalle);
                });

            migrationBuilder.CreateTable(
                name: "ChalecosTipos",
                columns: table => new
                {
                    IdChalecoTipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalecosTipos", x => x.IdChalecoTipo);
                });

            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    IdDireccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoDireccion = table.Column<int>(type: "integer", nullable: false),
                    Pais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Localidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Calle = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Cruce1 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Cruce2 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Apartamento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Manzana = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Solar = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Ruta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Km = table.Column<decimal>(type: "numeric", nullable: true),
                    Latitud = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitud = table.Column<decimal>(type: "numeric", nullable: true),
                    CodigoPostal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.IdDireccion);
                });

            migrationBuilder.CreateTable(
                name: "EsposasMarcas",
                columns: table => new
                {
                    IdEsposasMarca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsposasMarcas", x => x.IdEsposasMarca);
                });

            migrationBuilder.CreateTable(
                name: "EsposasModelos",
                columns: table => new
                {
                    IdEsposasModelo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsposasModelos", x => x.IdEsposasModelo);
                });

            migrationBuilder.CreateTable(
                name: "EsposasPoliciales",
                columns: table => new
                {
                    IdEsposasPolicial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEsposasTipo = table.Column<int>(type: "integer", nullable: false),
                    IdEsposasMarca = table.Column<int>(type: "integer", nullable: false),
                    IdEsposasModelo = table.Column<int>(type: "integer", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsposasPoliciales", x => x.IdEsposasPolicial);
                });

            migrationBuilder.CreateTable(
                name: "EsposasTipos",
                columns: table => new
                {
                    IdEsposasTipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EsposasTipos", x => x.IdEsposasTipo);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                });

            migrationBuilder.CreateTable(
                name: "ArmasModelos",
                columns: table => new
                {
                    IdArmaModelo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdArmaTipo = table.Column<int>(type: "integer", nullable: false),
                    IdArmaMarca = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmasModelos", x => x.IdArmaModelo);
                    table.ForeignKey(
                        name: "FK_ArmasModelos_ArmasMarcas_IdArmaMarca",
                        column: x => x.IdArmaMarca,
                        principalTable: "ArmasMarcas",
                        principalColumn: "IdArmaMarca",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmasModelos_ArmasTipos_IdArmaTipo",
                        column: x => x.IdArmaTipo,
                        principalTable: "ArmasTipos",
                        principalColumn: "IdArmaTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArmasPoliciales",
                columns: table => new
                {
                    IdArmaPolicial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdArmaTipo = table.Column<int>(type: "integer", nullable: false),
                    IdArmaMarca = table.Column<int>(type: "integer", nullable: false),
                    IdArmaModelo = table.Column<int>(type: "integer", nullable: false),
                    Serie = table.Column<string>(type: "text", nullable: true),
                    Numero = table.Column<string>(type: "text", nullable: true),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdArmaEstado = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmasPoliciales", x => x.IdArmaPolicial);
                    table.ForeignKey(
                        name: "FK_ArmasPoliciales_ArmasEstados_IdArmaEstado",
                        column: x => x.IdArmaEstado,
                        principalTable: "ArmasEstados",
                        principalColumn: "IdArmaEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmasPoliciales_ArmasMarcas_IdArmaMarca",
                        column: x => x.IdArmaMarca,
                        principalTable: "ArmasMarcas",
                        principalColumn: "IdArmaMarca",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmasPoliciales_ArmasModelos_IdArmaModelo",
                        column: x => x.IdArmaModelo,
                        principalTable: "ArmasModelos",
                        principalColumn: "IdArmaModelo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmasPoliciales_ArmasTipos_IdArmaTipo",
                        column: x => x.IdArmaTipo,
                        principalTable: "ArmasTipos",
                        principalColumn: "IdArmaTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FuncionariosEquipoPolicial",
                columns: table => new
                {
                    IdFuncionarioEquipoPolicial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IdArmaPolicial = table.Column<int>(type: "integer", nullable: false),
                    IdChalecoAntibalaPolicial = table.Column<int>(type: "integer", nullable: false),
                    IdEsposasPolicial = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionariosEquipoPolicial", x => x.IdFuncionarioEquipoPolicial);
                    table.ForeignKey(
                        name: "FK_FuncionariosEquipoPolicial_ArmasPoliciales_IdArmaPolicial",
                        column: x => x.IdArmaPolicial,
                        principalTable: "ArmasPoliciales",
                        principalColumn: "IdArmaPolicial",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuncionariosEquipoPolicial_ChalecosAntibalaPoliciales_IdCha~",
                        column: x => x.IdChalecoAntibalaPolicial,
                        principalTable: "ChalecosAntibalaPoliciales",
                        principalColumn: "IdChalecoAntibalaPolicial",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuncionariosEquipoPolicial_EsposasPoliciales_IdEsposasPolic~",
                        column: x => x.IdEsposasPolicial,
                        principalTable: "EsposasPoliciales",
                        principalColumn: "IdEsposasPolicial",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuncionariosEquipoPolicial_Usuarios_NombreUsuario",
                        column: x => x.NombreUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "NombreUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dependencias_IdDireccion",
                table: "Dependencias",
                column: "IdDireccion");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencias_IdTurno",
                table: "Dependencias",
                column: "IdTurno");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasEstados_Nombre",
                table: "ArmasEstados",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArmasMarcas_Nombre",
                table: "ArmasMarcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArmasModelos_IdArmaMarca",
                table: "ArmasModelos",
                column: "IdArmaMarca");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasModelos_IdArmaTipo",
                table: "ArmasModelos",
                column: "IdArmaTipo");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasPoliciales_IdArmaEstado",
                table: "ArmasPoliciales",
                column: "IdArmaEstado");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasPoliciales_IdArmaMarca",
                table: "ArmasPoliciales",
                column: "IdArmaMarca");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasPoliciales_IdArmaModelo",
                table: "ArmasPoliciales",
                column: "IdArmaModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasPoliciales_IdArmaTipo",
                table: "ArmasPoliciales",
                column: "IdArmaTipo");

            migrationBuilder.CreateIndex(
                name: "IX_ArmasTipos_Nombre",
                table: "ArmasTipos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChalecosEstados_Nombre",
                table: "ChalecosEstados",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChalecosMarcas_Nombre",
                table: "ChalecosMarcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChalecosModelos_Nombre",
                table: "ChalecosModelos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChalecosTalles_Nombre",
                table: "ChalecosTalles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChalecosTipos_Nombre",
                table: "ChalecosTipos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EsposasMarcas_Nombre",
                table: "EsposasMarcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EsposasModelos_Nombre",
                table: "EsposasModelos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EsposasTipos_Nombre",
                table: "EsposasTipos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEquipoPolicial_IdArmaPolicial",
                table: "FuncionariosEquipoPolicial",
                column: "IdArmaPolicial");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEquipoPolicial_IdChalecoAntibalaPolicial",
                table: "FuncionariosEquipoPolicial",
                column: "IdChalecoAntibalaPolicial");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEquipoPolicial_IdEsposasPolicial",
                table: "FuncionariosEquipoPolicial",
                column: "IdEsposasPolicial");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEquipoPolicial_NombreUsuario",
                table: "FuncionariosEquipoPolicial",
                column: "NombreUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_Nombre",
                table: "Turnos",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependencias_Direcciones_IdDireccion",
                table: "Dependencias",
                column: "IdDireccion",
                principalTable: "Direcciones",
                principalColumn: "IdDireccion",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependencias_Turnos_IdTurno",
                table: "Dependencias",
                column: "IdTurno",
                principalTable: "Turnos",
                principalColumn: "IdTurno",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependencias_UnidadesEjecutoras_IdUuee",
                table: "Dependencias",
                column: "IdUuee",
                principalTable: "UnidadesEjecutoras",
                principalColumn: "IdUuee",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dependencias_Direcciones_IdDireccion",
                table: "Dependencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Dependencias_Turnos_IdTurno",
                table: "Dependencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Dependencias_UnidadesEjecutoras_IdUuee",
                table: "Dependencias");

            migrationBuilder.DropTable(
                name: "ChalecosEstados");

            migrationBuilder.DropTable(
                name: "ChalecosMarcas");

            migrationBuilder.DropTable(
                name: "ChalecosModelos");

            migrationBuilder.DropTable(
                name: "ChalecosTalles");

            migrationBuilder.DropTable(
                name: "ChalecosTipos");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "EsposasMarcas");

            migrationBuilder.DropTable(
                name: "EsposasModelos");

            migrationBuilder.DropTable(
                name: "EsposasTipos");

            migrationBuilder.DropTable(
                name: "FuncionariosEquipoPolicial");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "ArmasPoliciales");

            migrationBuilder.DropTable(
                name: "ChalecosAntibalaPoliciales");

            migrationBuilder.DropTable(
                name: "EsposasPoliciales");

            migrationBuilder.DropTable(
                name: "ArmasEstados");

            migrationBuilder.DropTable(
                name: "ArmasModelos");

            migrationBuilder.DropTable(
                name: "ArmasMarcas");

            migrationBuilder.DropTable(
                name: "ArmasTipos");

            migrationBuilder.DropIndex(
                name: "IX_Dependencias_IdDireccion",
                table: "Dependencias");

            migrationBuilder.DropIndex(
                name: "IX_Dependencias_IdTurno",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "IdDireccion",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "IdTurno",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "NombreUsuarioJefe",
                table: "Dependencias");

            migrationBuilder.DropColumn(
                name: "NombreUsuarioSegundoJefe",
                table: "Dependencias");

            migrationBuilder.AlterColumn<string>(
                name: "Siglas",
                table: "Dependencias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Dependencias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddForeignKey(
                name: "FK_Dependencias_UnidadesEjecutoras_IdUuee",
                table: "Dependencias",
                column: "IdUuee",
                principalTable: "UnidadesEjecutoras",
                principalColumn: "IdUuee",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
