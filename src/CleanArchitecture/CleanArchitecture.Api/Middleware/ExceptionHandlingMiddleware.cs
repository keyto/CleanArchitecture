//https://www.udemy.com/course/clean-architecture/learn/lecture/40759812#overview

// using System.ComponentModel.DataAnnotations;

using CleanArchitecture.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> loger)
        {
            _next = next;
            _logger = loger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                // si no sucede error , que continue
                await _next(context);
                

            }
            catch (Exception ex)
            {
                // si dentro de la ejecucion del request se produce alguna excepcion dentro del programa
                _logger.LogError(ex, "Ocurrio una exception: {Message}",ex.Message);

                // obtener detalles de excepcion
                var exceptionDetails = GetExceptionDetails(ex);

                var problemDetails = new ProblemDetails { 
                    Status = exceptionDetails.Status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail= exceptionDetails.Detail
                };

                if (exceptionDetails.Errors is not null)
                {
                    problemDetails.Extensions["erros"] = exceptionDetails.Errors;
                }

                context.Response.StatusCode = exceptionDetails.Status;

                // devuelvo el objeto problemDetails en el request
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            
        }

        private static ExceptionDetails GetExceptionDetails(Exception exception)
        {
            // evaluar el tipo de excepcion
            return exception switch
            {
                ValidationExceptions validationExcepcion => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "Validacion de error",
                    "Han ocurrido uno o mas errores de validacion",
                    validationExcepcion.Errors
                ),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "ServerError",
                    "Error de Servidor",
                    "Error inesperado en la app",
                    null
                )
            };
        }

        internal record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            String Detail,
            IEnumerable<object>? Errors
        );
    }
}
