using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{

    public sealed class PermissionConfiguracion : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // definir configuraciones tecnicas de clase User al crearse en la DB

            // nombre de la tabla
            builder.ToTable("permissions");

            // clave primaria
            builder.HasKey(r => r.Id);

            // el Id  es un Ov PermissionId, transformarlo en un primitivo
            // UserId es un ov, hay que convertirlo a primitivo
            builder.Property(x => x.Id)
                .HasConversion(
                    permisionId => permisionId!.Value, 
                    value => new PermissionId(value)
                );

            // la propiedad Nombre tambien es un OV
            builder.Property(x => x.Nombre)
               .HasConversion(
                   n => n!.Value,
                   value => new Nombre(value)
               );

            // Carga de datos : Se encuentran en PermissionEnum
            // leer la enumeracion
            IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
                .Select(p => new Permission(                    // que voy a recuperar (obejtos tipo Permission) se especifica en el Select
                            new PermissionId((int)p),
                            new Nombre(p.ToString())
                ));              

            // añadimos datos de prueba a la tabla
            builder.HasData(permissions);


        }
    }
}
