using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
                                                AuthorizationHandlerContext context, 
                                                PermissionRequirement requirement)
        {
            // evaluar si en el token existe un usuario
            // El context es el jwt
            string? userId = context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            // si user es nulo, token invalido
            if (userId  is null)
            {
                //token invalido!
                return Task.CompletedTask;
            }

            // si existe usuario evaluar el permiso
            // extraer los claims de tipo CustomClaims.Permissions 
            // y crear una lista de hashset con los values ('Read','Write')
            HashSet<String> permissions = context.User.Claims
                .Where(x=>x.Type == CustomClaims.Permissions)
                .Select(x=> x.Value).ToHashSet();

            // si dentro de la coleccion de permisos del jwt existe el permiso pasado como parametro
            // //desde el controller " [HasPermissionAttibutes(PermissionEnum.ReadUser)]"
            if (permissions.Contains(requirement.Permission))
            {
                // todo ok !
                context.Succeed(requirement);
            }
            //token invalido!
            return Task.CompletedTask;
        }
    }
}
