using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Alquileres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Se hace con Dapper. Instalar el Nuget*/

namespace CleanArchitecture.Application.Alquileres.GetAlquiler
{
    // va a devolver una clase de tipo AlquilerResponse
    public sealed record GetAlquilerQuery(Guid AlquilerId):IQuery<AlquilerResponse>;
}
