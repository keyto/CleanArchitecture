using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Users
{
    public sealed class UserRole
    {
        public int RoleId { get; set; }
        public UserId? UserId { get; set; }
    }
}
