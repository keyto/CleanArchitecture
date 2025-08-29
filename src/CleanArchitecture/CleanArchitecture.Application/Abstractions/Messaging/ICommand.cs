using CleanArchitecture.Domain.Abstractions;
using MediatR;              /*IRequest*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    /*No se quiere devolver nada, solo se quiere hacer inserciones , actualiazciones etc..*/
    public interface ICommand  : IRequest<Result> , IBaseCommand
    {

    }

    /*Comando que si devuelve algun valor, no es ni lo recomendable ni lo normal*/
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {

    }


}
