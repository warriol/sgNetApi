using Microsoft.EntityFrameworkCore;
using sgNetApi.Domain.Entities;

namespace sgNetApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Mapeo de Tablas
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Grado> Grados => Set<Grado>();
    public DbSet<Escalafon> Escalafones => Set<Escalafon>();
    public DbSet<UnidadEjecutora> UnidadesEjecutoras => Set<UnidadEjecutora>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<HistorialUsuario> HistorialesUsuarios => Set<HistorialUsuario>();
    public DbSet<HistorialPassword> HistorialesPasswords => Set<HistorialPassword>();
    public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Usuario (CI como Llave Primaria no autoincremental)
        modelBuilder.Entity<Usuario>(b =>
            {
                b.HasKey(u => u.Ci);
                b.Property(u => u.Ci).ValueGeneratedNever();
                b.HasIndex(u => u.Correo).IsUnique();

                // Indicar explícitamente las claves foráneas para evitar campos duplicados
                b.HasOne(u => u.Grado)
                .WithMany()
                .HasForeignKey(u => u.IdGrado);

                b.HasOne(u => u.Escalafon)
                .WithMany()
                .HasForeignKey(u => u.IdEscalafon);

                b.HasOne(u => u.UnidadEjecutora)
                .WithMany()
                .HasForeignKey(u => u.IdUuee);

                b.HasOne(u => u.Dependencia)
                .WithMany()
                .HasForeignKey(u => new { u.IdDependencia, u.IdUuee });
            });

        // 2. Dependencia tiene Llave Primaria Compuesta (IdDependencia + IdUuee)
        modelBuilder.Entity<Dependencia>(b =>
        {
            b.HasKey(d => new { d.IdDependencia, d.IdUuee });

            b.HasOne(d => d.UnidadEjecutora)
            .WithMany()
            .HasForeignKey(d => d.IdUuee);
        });

        // 3. Configuración de Tablas Intermedias (Llaves compuestas)
        modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => new { rp.IdRol, rp.IdPermiso });

        modelBuilder.Entity<UsuarioRol>()
            .HasKey(ur => new { ur.UsuarioCi, ur.IdRol });

        modelBuilder.Entity<UsuarioPermiso>()
            .HasKey(up => new { up.UsuarioCi, up.IdPermiso });

        // 3. Claves primarias autoincrementales para catálogos e historiales
        modelBuilder.Entity<Grado>().HasKey(g => g.IdGrado);
        modelBuilder.Entity<Escalafon>().HasKey(e => e.IdEscalafon);
        modelBuilder.Entity<UnidadEjecutora>().HasKey(u => u.IdUuee);
        modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);
        modelBuilder.Entity<Permiso>().HasKey(p => p.IdPermiso);
        modelBuilder.Entity<HistorialUsuario>().HasKey(h => h.IdHistorial);
        modelBuilder.Entity<HistorialPassword>().HasKey(hp => hp.IdHistorialPassword);
    }
}