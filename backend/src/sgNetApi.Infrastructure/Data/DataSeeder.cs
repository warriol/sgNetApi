using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.Entities;
using sgNetApi.Domain.Interfaces;

namespace sgNetApi.Infrastructure.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        // 1. Semilla de Catálogos Cobre
        if (!await _context.Grados.AnyAsync())
        {
            _context.Grados.AddRange(
                new Grado { IdGrado = 1, Numero = 1, Texto = "Agente", Abreviatura = "Agte." },
                new Grado { IdGrado = 2, Numero = 2, Texto = "Cabo", Abreviatura = "Cbo." },
                new Grado { IdGrado = 3, Numero = 3, Texto = "Sargento", Abreviatura = "Sgt." },
                new Grado { IdGrado = 4, Numero = 4, Texto = "Suboficial Mayor", Abreviatura = "S.O.M." },
                new Grado { IdGrado = 5, Numero = 5, Texto = "Oficial Ayudante", Abreviatura = "Of. Aydte." },
                new Grado { IdGrado = 6, Numero = 6, Texto = "Oficial Principal", Abreviatura = "Of. Ppal." },
                new Grado { IdGrado = 7, Numero = 7, Texto = "Subcomisario", Abreviatura = "Sub Crio." },
                new Grado { IdGrado = 8, Numero = 8, Texto = "Comisario", Abreviatura = "Crio." },
                new Grado { IdGrado = 9, Numero = 9, Texto = "Comisario Mayor", Abreviatura = "Crio. My." },
                new Grado { IdGrado = 10, Numero = 10, Texto = "Comisario General", Abreviatura = "Crio. Gral." }
            );
        }

        if (!await _context.Escalafones.AnyAsync())
        {
            _context.Escalafones.AddRange(
                new Escalafon { IdEscalafon = 1, Nombre = "Ejecutivo", Abreviatura = "E" },
                new Escalafon { IdEscalafon = 2, Nombre = "Administrativo", Abreviatura = "A" },
                new Escalafon { IdEscalafon = 3, Nombre = "Técnico Profesional", Abreviatura = "PT" },
                new Escalafon { IdEscalafon = 4, Nombre = "Especializado", Abreviatura = "PE" },
                new Escalafon { IdEscalafon = 5, Nombre = "Servicios Generales", Abreviatura = "S" }
            );
        }

        if (!await _context.UnidadesEjecutoras.AnyAsync())
        {
            _context.UnidadesEjecutoras.Add(
                new UnidadEjecutora { IdUuee = 1, Nombre = "Dirección General", Siglas = "DG" }
            );
        }

        await _context.SaveChangesAsync();

        if (!await _context.Dependencias.AnyAsync())
        {
            _context.Dependencias.Add(
                new Dependencia
                {
                    IdDependencia = 1,
                    IdUuee = 1,
                    Nombre = "Centro de Cómputos y Tecnología",
                    Siglas = "CCT"
                }
            );
            await _context.SaveChangesAsync();
        }

        // 2. Semilla de Permisos Iniciales
        if (!await _context.Permisos.AnyAsync())
        {
            _context.Permisos.AddRange(
                new Permiso { IdPermiso = 1, Nombre = "admin.usuarios.crear", Descripcion = "Permite registrar nuevos usuarios" },
                new Permiso { IdPermiso = 2, Nombre = "admin.usuarios.leer", Descripcion = "Permite ver la lista y detalle de usuarios" },
                new Permiso { IdPermiso = 3, Nombre = "admin.usuarios.editar", Descripcion = "Permite modificar datos de usuarios" },
                new Permiso { IdPermiso = 4, Nombre = "admin.usuarios.eliminar", Descripcion = "Permite deshabilitar usuarios" },
                new Permiso { IdPermiso = 5, Nombre = "roles.administrar", Descripcion = "Permite gestionar roles y asignar permisos" }
            );
            await _context.SaveChangesAsync();
        }

        // 3. Semilla de Roles Iniciales y vinculación con Permisos
        if (!await _context.Roles.AnyAsync())
        {
            var rolAdmin = new Rol { IdRol = 1, Nombre = "Administrador" };
            var rolOperador = new Rol { IdRol = 2, Nombre = "Operador" };

            _context.Roles.AddRange(rolAdmin, rolOperador);
            await _context.SaveChangesAsync();

            // Solo asignar al Administrador los permisos cuyo nombre comience con "admin." o "usuarios."
            var permisosAdmin = await _context.Permisos
                .Where(p => p.Nombre.StartsWith("admin."))
                .ToListAsync();

            foreach (var permiso in permisosAdmin)
            {
                _context.Set<RolPermiso>().Add(new RolPermiso
                {
                    IdRol = rolAdmin.IdRol,
                    IdPermiso = permiso.IdPermiso
                });
            }
            await _context.SaveChangesAsync();
        }

        // 4. Semilla de Usuario Administrador Inicial
        if (!await _context.Usuarios.AnyAsync())
        {
            // Generar Hash y Salt de la contraseña por defecto para el primer acceso
            _passwordHasher.CrearPasswordHash("Admin.123456", out byte[] passwordHash, out byte[] passwordSalt);

            var usuarioAdmin = new Usuario
            {
                Ci = 43791806,
                NombreUsuario = "43791806",
                Nombre = "Wilson Denis",
                Apellido = "Arriola",
                Correo = "wilson.arriola@sgnet.com.uy",
                Celular = 099000000,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IntentosFallidos = 0,
                Habilitado = true,
                ExpiradoPorInactividad = false,
                Creado = DateTime.UtcNow,
                UltimoAcceso = DateTime.UtcNow,
                IdGrado = 10,       // Comisario General
                IdEscalafon = 3,   // Técnico Profesional
                IdUuee = 1,
                IdDependencia = 1
            };

            _context.Usuarios.Add(usuarioAdmin);
            await _context.SaveChangesAsync();

            // Asignar Rol Administrador al Usuario inicial
            _context.Set<UsuarioRol>().Add(new UsuarioRol
            {
                UsuarioCi = usuarioAdmin.Ci,
                IdRol = 1
            });

            // Registrar en el Historial de Usuarios la creación inicial
            _context.HistorialesUsuarios.Add(new HistorialUsuario
            {
                Fecha = DateTime.UtcNow,
                TipoAccion = "CREACION_INICIAL",
                Observaciones = "Usuario Administrador creado automáticamente por el sistema Seeder.",
                RealizadoPor = "SISTEMA",
                UsuarioCi = usuarioAdmin.Ci
            });

            // Registrar la primera contraseña en el historial de contraseñas
            _context.HistorialesPasswords.Add(new HistorialPassword
            {
                PasswordHash = passwordHash,
                FechaCreacion = DateTime.UtcNow,
                UsuarioCi = usuarioAdmin.Ci
            });

            await _context.SaveChangesAsync();
        }
    }
}