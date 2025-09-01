using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
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
    internal sealed class AlquilerConfiguration : IEntityTypeConfiguration<Alquiler>
    {
        public void Configure(EntityTypeBuilder<Alquiler> builder)
        {
            // definir configuraciones tecnicas de clase Alquiler al crearse en la DB

            // nombre de la tabla
            builder.ToTable("alquileres");

            // clave primaria
            builder.HasKey(alquiler => alquiler.Id);
            // para convertir la propiedad AlquilerId en un dato conocido por la bd
            builder.Property(alquiler => alquiler.Id)
                .HasConversion(AlquilerId => AlquilerId!.Value, value => new AlquilerId(value));

            // transformar un OV a valor primitivo
            builder.OwnsOne(alquiler => alquiler.PrecioPorPeriodo, precioBuilder =>
            {
                precioBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(TipoMoneda => TipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder.OwnsOne(alquiler => alquiler.Mantenimiento, precioBuilder =>
            {
                precioBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(TipoMoneda => TipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder.OwnsOne(alquiler => alquiler.Accesorios, precioBuilder =>
            {
                precioBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(TipoMoneda => TipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder.OwnsOne(alquiler => alquiler.PrecioTotal, precioBuilder =>
            {
                precioBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(TipoMoneda => TipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder.OwnsOne(alquiler => alquiler.Duracion);

            builder.HasOne<Vehiculo>()
                .WithMany()
                .HasForeignKey(alquiler => alquiler.VehiculoId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(alquiler =>alquiler.UserId);
                
        }
    }
}
