/*Clase 37 : https://www.udemy.com/course/clean-architecture/learn/lecture/40747610#overview*/
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using FluentValidation;         /*IValidator*/
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Behaviors
{
    /*Clase Cross Cutting Concern referiado a la validacion del Request Command. Utilizando el paquete de FluentValidation.DependencyInjectionExtension*/
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {

        // inyectar coleccion de objetos IValidator
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Si no existe ninguna validacion... continuar
            if (!_validators.Any()) 
            {
                // Que el request continue a los siguientes componentes
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
                .Select(validators => validators.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage
                )).ToList();

            if (validationErrors.Any())
            {
                throw new Exceptions.ValidationExceptions(validationErrors);
            }

            // Que el request continue a los siguientes componentes
            return await next();
        }
    }
}
