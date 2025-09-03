using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    public record RegisterUserRequest (string Email,string Nombre,string Apellidos,string Password);
}
