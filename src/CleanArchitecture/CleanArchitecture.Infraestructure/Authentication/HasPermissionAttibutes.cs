using CleanArchitecture.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Authentication
{
    public class HasPermissionAttibutes : AuthorizeAttribute
    {
        public HasPermissionAttibutes(PermissionEnum permission) 
            : base(policy: permission.ToString())
        {

            
        }
    }
}
