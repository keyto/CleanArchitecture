using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Roles
{
    public sealed class Role : Enumeration<Role>
    {
        public static readonly Role Cliente = new Role(1,"Cliente");
        public static readonly Role Admin = new Role(2, "Admin");

        public Role(int id, string name) : base(id, name)
        {
            
        }

        public ICollection<Permission>? Permissions { get; set; }
    }
}
