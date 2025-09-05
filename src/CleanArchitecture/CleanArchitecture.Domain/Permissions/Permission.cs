using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Permissions
{
    public sealed class Permission : Entity<PermissionId>
    {
        public Nombre? Nombre { get; init; }

        private Permission()
        {

        }

        public Permission(Nombre nombre) : base()
        {
            
        }

        public Permission(PermissionId id, Nombre nombre) : base(id)
        {
            Nombre = nombre;
        }

        public static Result<Permission> Create(Nombre nombre) 
        {
            return new Permission(nombre);
        }
       
    }
}
