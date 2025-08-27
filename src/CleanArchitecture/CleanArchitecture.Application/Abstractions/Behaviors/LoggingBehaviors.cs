/*Video 36 : https://www.udemy.com/course/clean-architecture/learn/lecture/40747604#overview*/
using CleanArchitecture.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Instalar Nuget Microsoft.Extensions.Logging.Abstractions 8.0*/

namespace CleanArchitecture.Application.Abstractions.Behaviors
{
    /*Solo se aplica el Log para modificacion de datos "where TRequest : IBaseCommand"*/
    /*Clase Cross Cutting Concern referido al log*/

    
    public class LoggingBehaviors<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviors(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // se podria utilizar SeriLog pero vamos a trabajar con el propio de Microsoft
            var name = request.GetType().Name;  // devolver el nombre de la clase del Command

            try
            {
                _logger.LogInformation($"Ejecutando el Command Request {name}" );
                var result = await next();      // ver el resultado de la ejecucion
                _logger.LogInformation($"El comando {name} se ejecuto exitosamente");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"El comando {name} tuvo errores");
                throw; 
            }
        }
    }
}
