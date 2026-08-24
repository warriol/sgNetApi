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
    public DbSet<Nacionalidad> Nacionalidades => Set<Nacionalidad>();
    public DbSet<EstadoCivil> EstadosCiviles => Set<EstadoCivil>();
    public DbSet<Profesion> Profesiones => Set<Profesion>();
    public DbSet<Grado> Grados => Set<Grado>();
    public DbSet<Escalafon> Escalafones => Set<Escalafon>();
    public DbSet<UnidadEjecutora> UnidadesEjecutoras => Set<UnidadEjecutora>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<HistorialUsuario> HistorialesUsuarios => Set<HistorialUsuario>();
    public DbSet<HistorialPassword> HistorialesPasswords => Set<HistorialPassword>();
    public DbSet<AuditoriaLog> AuditoriaLogs => Set<AuditoriaLog>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(b =>
        {
            b.HasKey(u => u.NombreUsuario);
            b.Property(u => u.NombreUsuario).HasMaxLength(50);
            b.Property(u => u.Ci).HasColumnType("bigint");
            b.HasIndex(u => u.Ci).IsUnique();
            b.HasIndex(u => u.Correo).IsUnique();
            b.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
            b.Property(u => u.Apellido).HasMaxLength(100).IsRequired();
            b.Property(u => u.Correo).HasMaxLength(150).IsRequired();
            b.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
            b.HasOne(u => u.Dependencia).WithMany().HasForeignKey(u => u.IdDependencia).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(u => u.Nacionalidad).WithMany(n => n.Usuarios).HasForeignKey(u => u.IdNacionalidad).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(u => u.EstadoCivil).WithMany().HasForeignKey(u => u.IdEstadoCivil);
            b.HasOne(u => u.Profesion).WithMany().HasForeignKey(u => u.IdProfesion);
            b.HasOne(u => u.Grado).WithMany().HasForeignKey(u => u.IdGrado);
            b.HasOne(u => u.Escalafon).WithMany().HasForeignKey(u => u.IdEscalafon);
        });

        modelBuilder.Entity<Dependencia>(b =>
        {
            b.HasKey(d => d.IdDependencia);

            b.HasOne(d => d.UnidadEjecutora)
            .WithMany()
            .HasForeignKey(d => d.IdUuee);
        });

        modelBuilder.Entity<Nacionalidad>(b => { b.HasKey(x => x.IdNacionalidad); b.HasIndex(x => x.Nombre).IsUnique(); b.HasIndex(x => x.CodigoIso).IsUnique(); });
        modelBuilder.Entity<EstadoCivil>(b => { b.HasKey(x => x.IdEstadoCivil); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<Profesion>(b => { b.HasKey(x => x.IdProfesion); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<AuditoriaLog>(b => { b.HasKey(x => x.IdLog); b.Property(x => x.IpOrigen).HasMaxLength(45).IsRequired(); b.Property(x => x.MetodoHttp).HasMaxLength(10).IsRequired(); b.Property(x => x.Ruta).HasMaxLength(500).IsRequired(); });

        // 3. Configuración de Tablas Intermedias (Llaves compuestas)
        modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => new { rp.IdRol, rp.IdPermiso });
        modelBuilder.Entity<RolPermiso>().HasOne(x => x.Rol).WithMany(x => x.RolPermisos).HasForeignKey(x => x.IdRol);
        modelBuilder.Entity<RolPermiso>().HasOne(x => x.Permiso).WithMany(x => x.RolPermisos).HasForeignKey(x => x.IdPermiso);

        modelBuilder.Entity<UsuarioRol>().HasKey(ur => new { ur.NombreUsuario, ur.IdRol });
        modelBuilder.Entity<UsuarioRol>().HasOne(x => x.Usuario).WithMany(x => x.UsuarioRoles).HasForeignKey(x => x.NombreUsuario);
        modelBuilder.Entity<UsuarioRol>().HasOne(x => x.Rol).WithMany(x => x.UsuarioRoles).HasForeignKey(x => x.IdRol);

        modelBuilder.Entity<UsuarioPermiso>().HasKey(up => new { up.NombreUsuario, up.IdPermiso });
        modelBuilder.Entity<UsuarioPermiso>().HasOne(x => x.Usuario).WithMany(x => x.UsuarioPermisos).HasForeignKey(x => x.NombreUsuario);
        modelBuilder.Entity<UsuarioPermiso>().HasOne(x => x.Permiso).WithMany(x => x.UsuarioPermisos).HasForeignKey(x => x.IdPermiso);

        modelBuilder.Entity<HistorialUsuario>().HasOne(x => x.Usuario).WithMany(x => x.Historiales).HasForeignKey(x => x.UsuarioNombreUsuario).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<HistorialPassword>().HasOne(x => x.Usuario).WithMany(x => x.HistorialPasswords).HasForeignKey(x => x.UsuarioNombreUsuario).OnDelete(DeleteBehavior.Restrict);

        // 3. Claves primarias autoincrementales para catálogos e historiales
        modelBuilder.Entity<Grado>().HasKey(g => g.IdGrado);
        modelBuilder.Entity<Escalafon>().HasKey(e => e.IdEscalafon);
        modelBuilder.Entity<UnidadEjecutora>().HasKey(u => u.IdUuee);
        modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);
        modelBuilder.Entity<Rol>().Property(r => r.Nombre).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Rol>().Property(r => r.Descripcion).HasMaxLength(255);
        modelBuilder.Entity<Rol>().HasIndex(r => r.Nombre).IsUnique();
        modelBuilder.Entity<Permiso>().Property(p => p.Nombre).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Permiso>().Property(p => p.Descripcion).HasMaxLength(255).IsRequired();
        modelBuilder.Entity<Permiso>().HasIndex(p => p.Nombre).IsUnique();
        modelBuilder.Entity<Permiso>().HasKey(p => p.IdPermiso);
        modelBuilder.Entity<HistorialUsuario>().HasKey(h => h.IdHistorial);
        modelBuilder.Entity<HistorialPassword>().HasKey(hp => hp.IdHistorialPassword);
    }
}