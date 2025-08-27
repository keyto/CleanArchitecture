using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Configurations
{
    internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
    {
        public void Configure(EntityTypeBuilder<Vehiculo> builder)
        {
            // definir configuraciones tecnicas de clase vehiculo al crearse en la DB

            // nombre de la tabla
            builder.ToTable("vehiculos");
            
            // clave primaria
            builder.HasKey(vehiculo => vehiculo.Id);

            // contiene un Object Value (direccion)
            // Esto significa que en la bd,la direccion se añadira tambien en la tabla Vehiculo,
            // aunque se haya creado una entidad de tipo Direccion
            // Si el OV tiene mas de 1 propiedad hay que usar OwnsOne 
            builder.OwnsOne(v => v.Direccion);

            builder.OwnsOne(v => v.Precio , priceBuilder => {
                priceBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(tipoMoneda => tipoMoneda.Codigo , codigo => TipoMoneda.FromCodigo(codigo!));
            });

            builder.OwnsOne(v => v.Mantenimiento , priceBuilder => {
                priceBuilder.Property(moneda => moneda.TipoMoneda)
               .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
            });

            // Tamaño maximo del campo Modelo,
            // Pero hay un problema,Modelo es un OV (clase Modelo)
            // Para eso se convierte en un tipo primitivo de tipo string.
            // Si el OV tiene solo 1 propiedad se convierte directamente a string
            builder.Property(v=>v.Modelo)
                .HasMaxLength(200)
                .HasConversion(modelo=> modelo!.Value , value => new Modelo(value));

            builder.Property(v => v.Vin)
               .HasMaxLength(500)
               .HasConversion(vin => vin!.Value, value => new Vin(value));


            // esta columna se añade para que pueda trabajar la Concurrencia de alquileres,
            // para que no se solapen accciones de alquiler por ejemplo.
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
