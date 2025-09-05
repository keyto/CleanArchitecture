using CleanArchitecture.Domain.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Roles
{
    public sealed class RolePermission
    {
        public int RoleId { get; set; }

        public PermissionId? PermissionId { get; set; }
    }
}
