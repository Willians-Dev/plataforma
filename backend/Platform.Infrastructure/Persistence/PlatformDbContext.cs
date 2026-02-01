using Microsoft.EntityFrameworkCore;
using Platform.Domain.Security.Entities;

namespace Platform.Infrastructure.Persistence
{
    public class PlatformDbContext : DbContext
    {
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RoleModule> RoleModules => Set<RoleModule>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("seguridad");

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("usuarios");
                e.HasKey(x => x.Id);
                e.Property(x => x.Username).HasColumnName("usuario").IsRequired();
                e.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
                e.Property(x => x.FirstName).HasColumnName("nombre").IsRequired();
                e.Property(x => x.LastName).HasColumnName("apellido").IsRequired();
                e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
                e.HasIndex(x => x.Username).IsUnique();
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("roles");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasColumnName("nombre").IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<Module>(e =>
            {
                e.ToTable("modulos");
                e.HasKey(x => x.Id);
                e.Property(x => x.Code).HasColumnName("codigo").IsRequired();
                e.Property(x => x.Name).HasColumnName("nombre").IsRequired();
                e.Property(x => x.Description).HasColumnName("descripcion");
                e.Property(x => x.Icon).HasColumnName("icono").IsRequired();
                e.Property(x => x.Route).HasColumnName("ruta").IsRequired();
                e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
                e.Property(x => x.Order).HasColumnName("orden");
                e.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.ToTable("usuario_roles");
                e.HasKey(x => new { x.UserId, x.RoleId });
                e.Property(x => x.UserId).HasColumnName("usuario_id");
                e.Property(x => x.RoleId).HasColumnName("rol_id");
            });

            modelBuilder.Entity<RoleModule>(e =>
            {
                e.ToTable("rol_modulos");
                e.HasKey(x => new { x.RoleId, x.ModuleId });
                e.Property(x => x.RoleId).HasColumnName("rol_id");
                e.Property(x => x.ModuleId).HasColumnName("modulo_id");
            });
        }
    }
}