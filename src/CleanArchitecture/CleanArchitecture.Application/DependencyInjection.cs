using CleanArchitecture.Application.Abstractions.Behaviors;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            /* Desde este metodo hay que registrar los servicios que vaya creando en esta capa Application.*/

            /*
             * Por ejemplo la comunicacion entre los Command, los Queries y sus respectivos Handlers utilizando el patron
             * Mediator y ejecutandose sobre el paquete MediaTr 
             */
            services.AddMediatR(configuration =>
                {
                    configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                    configuration.AddOpenBehavior(typeof(LoggingBehaviors<,>));
                    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));   /*Agregar validaciones a los objetos de tipo Command*/
                }
            );

            /*escanear todas las clases desde fluentValidacion y las inyecta automaticamente*/
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddTransient<CleanArchitecture.Domain.Alquileres.PrecioService>();       


            return services;

        }
    }
}
