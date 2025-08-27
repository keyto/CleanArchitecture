using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler
{
    public class ReservarAlquilerCommandValidator : AbstractValidator<ReservarAlquilerCommand>      // Evaluará a ver si hay errores en ReservarAlquilerCommand
    {
        public ReservarAlquilerCommandValidator()
        {
            // Si no se cumple alguna de estas reglas se añade esa validacion al Middleware 
            RuleFor(c => c.UserId).NotEmpty();          /*Validacion no vacio*/
            RuleFor(c => c.VehiculoId).NotEmpty();
            RuleFor(c => c.FechaInicio).LessThan(c=> c.FechaFin);

            /*Si no se cumple cada una de las condiciones, nos dara un ValidationError por cada validacion
             y posteriormente el handler MiddlerWare en Api capturara la excepcion y avisara al cliente*/
        }
    }
}
