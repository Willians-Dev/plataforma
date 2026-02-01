using Microsoft.EntityFrameworkCore;
using Platform.Domain.Security.Entities;
using Platform.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Infrastructure.Security
{
    public class SecuritySeeder
    {
        public static async Task SeedAsync(PlatformDbContext db)
        {
            if (await db.Users.AnyAsync())
                return;

            var adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            };

            var modules = new List<Module>
            {
                new Module
                {
                    Id = Guid.NewGuid(),
                    Code = "INVENTARIO",
                    Name = "Inventario",
                    Description = "Gestión de activos e inventario",
                    Icon = "inventory_2",
                    Route = "/inventario",
                    IsActive = true,
                    Order = 1
                },
                new Module
                {
                    Id = Guid.NewGuid(),
                    Code = "AUDITORIA",
                    Name = "Auditoria",
                    Description = "Monitoreo y Trazabilidad",
                    Icon = "fact_check",
                    Route = "/auditoria",
                    IsActive = true,
                    Order = 2
                },
                new Module
                {
                    Id = Guid.NewGuid(),
                    Code = "REPORTES",
                    Name = "Reportes",
                    Description = "Reportes operativos y ejecutivos",
                    Icon = "bar-chart",
                    Route = "/reportes",
                    IsActive = true,
                    Order = 3
                }
            };

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                FirstName = "Admin",
                LastName = "Platform",
                PasswordHash = PasswordHasher.Hash("admin123"),
                IsActive = true
            };

            db.Roles.Add(adminRole);
            db.Modules.AddRange(modules);
            db.Users.Add(adminUser);

            db.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });

            db.RoleModules.AddRange(modules.Select(m => new RoleModule
            {
                RoleId = adminRole.Id,
                ModuleId = m.Id
            }));

            await db.SaveChangesAsync();
        }
    }
}