using CleanArchitecture.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    // Cuando se registre un usuario se devolvera el GUID del usuario
    public sealed record RegisterUserCommand (
        string Email, 
        string Nombre, 
        string Apellidos, 
        string Password) : ICommand<Guid>;
   
}
