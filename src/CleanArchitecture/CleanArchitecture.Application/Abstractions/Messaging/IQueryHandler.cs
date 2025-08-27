using CleanArchitecture.Domain.Abstractions;
using MediatR;          /*IRequestHandler*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    public interface IQueryHandler<T, TResponse> 
        : IRequestHandler<T, Result<TResponse>> 
        where T : IQuery<TResponse>
    {

    }
}
