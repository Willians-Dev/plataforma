using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Platform.Domain.Security.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();
    }
}
