using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CleanArchitecture.Infraestructure
{
    public  sealed class ApplicationDbContext : DbContext , IUnitOfWork
    {
        private readonly IPublisher _publisher;
        

        public ApplicationDbContext(DbContextOptions options,IPublisher publisher) :base(options)
        {
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // se indica que se apliquen las configuraciones dentro del modelBuilder
            // Con esta linea nos aseguramos que se aplican las configuraciones programadas en el Assemby.
            // directorio Configurations. y despues aplicar las configuraciones al modelo de EF.
            // busca todas las clases en el assembly de Infrastructure que implementen IEntityTypeConfiguration<T>.
            // Ejemplo(dentro de Infrastructure/Configurations):
            // Es decir, EF creara las tablas en BD de las entidades que hereden de IEntityTypeConfiguration<T>.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            try
            {
                // 1. Guardar primero los cambios en la base de datos
                var result = await base.SaveChangesAsync(cancellation);

                // publicar domain events
                await PublishDomainEventsAsync();


                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {

                throw new ConcurrencyException("La excepcion por concurrencia se disparo",ex);
            }
        }

        private async Task PublishDomainEventsAsync()
        {
            // obtener todos los domain events que extiendan de la clase Entity
            var domainEvents = ChangeTracker        // El ChangeTracker es el que sigue los cambios de EF.
                .Entries<IEntity>()                  // Se buscan todas las entidades que heredan de EntityBase
                .Select(entry => entry.Entity)
                .SelectMany(entity =>               // De cada entidad, se sacan los eventos de dominio que tenga acumulados (ejemplo: VehiculoReservadoEvent).
                {
                    // 3. Limpiar los eventos de las entidades (ya que se van a despachar)
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                }).ToList();

            /* hace lo mismo que lo de arriba
             var domainEvents = ChangeTracker
                .Entries<EntityBase>() // todas las entidades rastreadas que heredan de EntityBase
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
            
                foreach (var entity in ChangeTracker.Entries<EntityBase>())
                {
                    entity.Entity.ClearDomainEvents();
                }
             */



            // 4. Publicar los eventos usando el publicador inyectado (ej: MediatR)
            foreach (var domainEvent in domainEvents) 
            {
                await _publisher.Publish(domainEvent);
            }

        }
    }
}
