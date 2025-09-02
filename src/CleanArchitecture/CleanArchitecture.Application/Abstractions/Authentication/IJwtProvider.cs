//https://www.udemy.com/course/clean-architecture-authentication/learn/lecture/41936894#overview 

using CleanArchitecture.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Authentication
{
    /// <summary>
    /// Servicio para la implementacion de la generacion del Jason Web Token
    /// </summary>
    public interface IJwtProvider
    {
        Task<string> Generate(User user);
    }
}
