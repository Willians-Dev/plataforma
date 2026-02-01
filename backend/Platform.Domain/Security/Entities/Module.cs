using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Domain.Security.Entities
{
    public class Module
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public int Order { get; set; }

        public ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();
    }
}
