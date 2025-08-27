using CleanArchitecture.Domain.Abstractions;
using MediatR;          /*IRequest*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Messaging
{
    public interface IQuery <TResponse> 
        : IRequest<Result<TResponse>>           /*Especifica que se devólverá un TResponse al ejecutarse el QueryHandler*/
    {

    }
}
