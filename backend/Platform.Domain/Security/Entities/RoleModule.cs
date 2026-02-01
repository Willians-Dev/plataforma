using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Domain.Security.Entities
{
    public class RoleModule
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Guid ModuleId { get; set; }
        public Module Module { get; set; } = null!;
    }
}
