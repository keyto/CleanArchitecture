using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // definir configuraciones tecnicas de clase User al crearse en la DB

            // nombre de la tabla
            builder.ToTable("users");

            // clave primaria
            builder.HasKey(user => user.Id);
            // para convertir la propiedad AlquilerId en un dato conocido por la bd
            builder.Property(user=> user.Id)
                .HasConversion(userId => userId!.Value, value => new UserId(value));


            builder.Property(u => u.Nombre)
               .HasMaxLength(200)
               .HasConversion(nombre => nombre!.Value, value => new Nombre(value));

            builder.Property(u => u.Apellido)
           .HasMaxLength(200)
           .HasConversion(apellido => apellido!.Value, value => new Apellido(value));

            builder.Property(u => u.Email)
            .HasMaxLength(200)
            .HasConversion(email=> email!.Value, value => new Domain.Users.Email(value));

            builder.HasIndex(u => u.Email).IsUnique();


        }
    }
}
