using CleanArchitecture.Domain.Abstractions;
using MediatR;          /*IRequestHandler*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    public interface ICommandHandler<T> : IRequestHandler<T, Result>  
    where T : ICommand
    {

    }

    public interface ICommandHandler<T, TResponse>
        : IRequestHandler<T, Result<TResponse>>
        where T : ICommand<TResponse>
    { 
    
    }
}
