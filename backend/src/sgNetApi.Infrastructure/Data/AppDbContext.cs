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
    public DbSet<Direccion> Direcciones => Set<Direccion>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<HistorialUsuario> HistorialesUsuarios => Set<HistorialUsuario>();
    public DbSet<HistorialPassword> HistorialesPasswords => Set<HistorialPassword>();
    public DbSet<AuditoriaLog> AuditoriaLogs => Set<AuditoriaLog>();
    public DbSet<ArmaTipo> ArmasTipos => Set<ArmaTipo>();
    public DbSet<ArmaMarca> ArmasMarcas => Set<ArmaMarca>();
    public DbSet<ArmaModelo> ArmasModelos => Set<ArmaModelo>();
    public DbSet<ArmaEstado> ArmasEstados => Set<ArmaEstado>();
    public DbSet<ArmaPolicial> ArmasPoliciales => Set<ArmaPolicial>();
    public DbSet<ChalecoTipo> ChalecosTipos => Set<ChalecoTipo>();
    public DbSet<ChalecoMarca> ChalecosMarcas => Set<ChalecoMarca>();
    public DbSet<ChalecoModelo> ChalecosModelos => Set<ChalecoModelo>();
    public DbSet<ChalecoTalle> ChalecosTalles => Set<ChalecoTalle>();
    public DbSet<ChalecoEstado> ChalecosEstados => Set<ChalecoEstado>();
    public DbSet<ChalecoAntibalaPolicial> ChalecosAntibalaPoliciales => Set<ChalecoAntibalaPolicial>();
    public DbSet<EsposasTipo> EsposasTipos => Set<EsposasTipo>();
    public DbSet<EsposasMarca> EsposasMarcas => Set<EsposasMarca>();
    public DbSet<EsposasModelo> EsposasModelos => Set<EsposasModelo>();
    public DbSet<EsposasPolicial> EsposasPoliciales => Set<EsposasPolicial>();
    public DbSet<FuncionarioEquipoPolicial> FuncionariosEquipoPolicial => Set<FuncionarioEquipoPolicial>();

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
            b.HasOne(u => u.Dependencia).WithMany(d => d.Funcionarios).HasForeignKey(u => u.IdDependencia).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(u => u.TurnoAsignado).WithMany().HasForeignKey(u => u.IdTurnoAsignado).OnDelete(DeleteBehavior.SetNull);
            b.HasOne(u => u.Nacionalidad).WithMany(n => n.Usuarios).HasForeignKey(u => u.IdNacionalidad).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(u => u.EstadoCivil).WithMany().HasForeignKey(u => u.IdEstadoCivil);
            b.HasOne(u => u.Profesion).WithMany().HasForeignKey(u => u.IdProfesion);
            b.HasOne(u => u.Grado).WithMany().HasForeignKey(u => u.IdGrado);
            b.HasOne(u => u.Escalafon).WithMany().HasForeignKey(u => u.IdEscalafon);
        });

        modelBuilder.Entity<Dependencia>(b =>
        {
            b.HasKey(d => d.IdDependencia);
            b.Property(d => d.Nombre).HasMaxLength(150).IsRequired();
            b.Property(d => d.Siglas).HasMaxLength(20).IsRequired();
            b.HasOne(d => d.UnidadEjecutora).WithMany().HasForeignKey(d => d.IdUuee).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(d => d.Direccion).WithMany().HasForeignKey(d => d.IdDireccion).OnDelete(DeleteBehavior.SetNull);
            b.HasOne(d => d.Turno).WithMany(t => t.Dependencias).HasForeignKey(d => d.IdTurno).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Direccion>(b =>
        {
            b.HasKey(d => d.IdDireccion);
            b.Property(d => d.Pais).HasMaxLength(100).IsRequired();
            b.Property(d => d.Departamento).HasMaxLength(100).IsRequired();
            b.Property(d => d.Localidad).HasMaxLength(100).IsRequired();
            b.Property(d => d.Calle).HasMaxLength(150);
            b.Property(d => d.Cruce1).HasMaxLength(150);
            b.Property(d => d.Cruce2).HasMaxLength(150);
            b.Property(d => d.Numero).HasMaxLength(20);
            b.Property(d => d.Apartamento).HasMaxLength(20);
            b.Property(d => d.Manzana).HasMaxLength(30);
            b.Property(d => d.Solar).HasMaxLength(30);
            b.Property(d => d.Ruta).HasMaxLength(100);
            b.Property(d => d.CodigoPostal).HasMaxLength(20);
            b.Property(d => d.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Turno>(b =>
        {
            b.HasKey(t => t.IdTurno);
            b.Property(t => t.Nombre).HasMaxLength(100).IsRequired();
            b.Property(t => t.Descripcion).HasMaxLength(255);
            b.HasIndex(t => t.Nombre).IsUnique();
        });

        modelBuilder.Entity<Nacionalidad>(b => { b.HasKey(x => x.IdNacionalidad); b.HasIndex(x => x.Nombre).IsUnique(); b.HasIndex(x => x.CodigoIso).IsUnique(); });
        modelBuilder.Entity<EstadoCivil>(b => { b.HasKey(x => x.IdEstadoCivil); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<Profesion>(b => { b.HasKey(x => x.IdProfesion); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<AuditoriaLog>(b => { b.HasKey(x => x.IdLog); b.Property(x => x.IpOrigen).HasMaxLength(45).IsRequired(); b.Property(x => x.MetodoHttp).HasMaxLength(10).IsRequired(); b.Property(x => x.Ruta).HasMaxLength(500).IsRequired(); });

        modelBuilder.Entity<ArmaTipo>(b => { b.HasKey(x => x.IdArmaTipo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ArmaMarca>(b => { b.HasKey(x => x.IdArmaMarca); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ArmaModelo>(b => { b.HasKey(x => x.IdArmaModelo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasOne(x => x.ArmaTipo).WithMany().HasForeignKey(x => x.IdArmaTipo).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.ArmaMarca).WithMany().HasForeignKey(x => x.IdArmaMarca).OnDelete(DeleteBehavior.Restrict); });
        modelBuilder.Entity<ArmaEstado>(b => { b.HasKey(x => x.IdArmaEstado); b.Property(x => x.Nombre).HasMaxLength(50).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ArmaPolicial>(b => { b.HasKey(x => x.IdArmaPolicial); b.HasOne(x => x.ArmaTipo).WithMany().HasForeignKey(x => x.IdArmaTipo).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.ArmaMarca).WithMany().HasForeignKey(x => x.IdArmaMarca).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.ArmaModelo).WithMany().HasForeignKey(x => x.IdArmaModelo).OnDelete(DeleteBehavior.Restrict); b.HasOne(x => x.ArmaEstado).WithMany().HasForeignKey(x => x.IdArmaEstado).OnDelete(DeleteBehavior.Restrict); });

        modelBuilder.Entity<ChalecoTipo>(b => { b.HasKey(x => x.IdChalecoTipo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ChalecoMarca>(b => { b.HasKey(x => x.IdChalecoMarca); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ChalecoModelo>(b => { b.HasKey(x => x.IdChalecoModelo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ChalecoTalle>(b => { b.HasKey(x => x.IdChalecoTalle); b.Property(x => x.Nombre).HasMaxLength(20).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ChalecoEstado>(b => { b.HasKey(x => x.IdChalecoEstado); b.Property(x => x.Nombre).HasMaxLength(50).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<ChalecoAntibalaPolicial>(b => { b.HasKey(x => x.IdChalecoAntibalaPolicial); b.Property(x => x.Color).HasMaxLength(50); });

        modelBuilder.Entity<EsposasTipo>(b => { b.HasKey(x => x.IdEsposasTipo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<EsposasMarca>(b => { b.HasKey(x => x.IdEsposasMarca); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<EsposasModelo>(b => { b.HasKey(x => x.IdEsposasModelo); b.Property(x => x.Nombre).HasMaxLength(100).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique(); });
        modelBuilder.Entity<EsposasPolicial>(b => { b.HasKey(x => x.IdEsposasPolicial); });

        modelBuilder.Entity<FuncionarioEquipoPolicial>(b =>
        {
            b.HasKey(x => x.IdFuncionarioEquipoPolicial);
            b.Property(x => x.NombreUsuario).HasMaxLength(50).IsRequired();
            b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.NombreUsuario).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.ArmaPolicial).WithMany().HasForeignKey(x => x.IdArmaPolicial).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.ChalecoAntibalaPolicial).WithMany().HasForeignKey(x => x.IdChalecoAntibalaPolicial).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(x => x.EsposasPolicial).WithMany().HasForeignKey(x => x.IdEsposasPolicial).OnDelete(DeleteBehavior.Restrict);
        });

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