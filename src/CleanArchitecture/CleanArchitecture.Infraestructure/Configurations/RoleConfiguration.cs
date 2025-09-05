using CleanArchitecture.Domain.Roles;
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // definir configuraciones tecnicas de clase User al crearse en la DB

            // nombre de la tabla
            builder.ToTable("role");

            // clave primaria
            builder.HasKey(r => r.Id);

            // Role.GetValues() devuelve todos los Roles
            builder.HasData(Role.GetValues());

            // relacion de muchos a muchos entre Roles y Permissions
            builder.HasMany(x => x.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();
        }
    }
}

 