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
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // definir configuraciones tecnicas de clase User al crearse en la DB

            // nombre de la tabla
            builder.ToTable("users_roles");

            // clave primaria compuesta por 2 campos
            builder.HasKey(x => new { x.RoleId ,x.UserId });

            // UserId es un ov, hay que convertirlo a primitivo
            builder.Property(user => user.UserId)
                .HasConversion(userId => userId!.Value, value => new UserId(value));

        }
    }
}
