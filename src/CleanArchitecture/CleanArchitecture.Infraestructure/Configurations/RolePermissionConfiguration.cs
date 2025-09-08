using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{
    public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RolePermission> builder)
        {
            // definir configuraciones tecnicas de clase User al crearse en la DB

            // nombre de la tabla
            builder.ToTable("roles_permissions");

            // clave primaria compuesta por 2 campos
            builder.HasKey(x => new { x.RoleId, x.PermissionId});

            // UserId es un ov, hay que convertirlo a primitivo
            builder.Property(x => x.PermissionId)
                .HasConversion(permisionId => permisionId!.Value, value => new PermissionId(value));

            // añadimos datos de prueba a la tabla
            builder.HasData(
                    Create(Role.Cliente,PermissionEnum.ReadUser) 
                    ,Create(Role.Admin, PermissionEnum.ReadUser) 
                    ,Create(Role.Admin, PermissionEnum.WriteUser) 
                    ,Create(Role.Admin, PermissionEnum.UpdateUser));

        }

        // datos de prueba para crear un RolePermission
        private static RolePermission Create(Role role, PermissionEnum permission)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = new PermissionId((int)permission)
            };
        }
    }
}
