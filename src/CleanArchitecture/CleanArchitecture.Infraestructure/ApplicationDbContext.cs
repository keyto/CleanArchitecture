using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            try
            {
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
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                }).ToList();

            // Publicar los eventos
            foreach (var domainEvent in domainEvents) 
            {
                await _publisher.Publish(domainEvent);
            }

        }
    }
}
