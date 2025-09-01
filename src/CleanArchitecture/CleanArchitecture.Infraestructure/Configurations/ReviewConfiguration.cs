using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // definir configuraciones tecnicas de clase Review al crearse en la DB

            // nombre de la tabla
            builder.ToTable("review");

            // clave primaria
            builder.HasKey(review => review.Id);
            
            // para convertir la propiedad AlquilerId en un dato conocido por la bd
            builder.Property(review => review.Id)
                .HasConversion(reviewId => reviewId!.Value, value => new ReviewId(value));


            builder.Property(review => review.Rating)
                .HasConversion(rating => rating!.Value, value => Rating.Create(value).Value);

            builder.Property(review => review.Comentario)
                .HasMaxLength(200)
                .HasConversion(comentario => comentario!.Value, value => new Comentario(value));

            // relacion entre review  y vehiculos es de 1 a muchos
            builder.HasOne<Vehiculo>()
                .WithMany()
                .HasForeignKey(review => review.VehiculoId);

            builder.HasOne<Alquiler>()
               .WithMany()
               .HasForeignKey(review => review.AlquilerId);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(review => review.UserId);

        }

      

    }
}
