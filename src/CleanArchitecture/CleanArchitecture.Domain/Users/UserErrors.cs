using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound = new Error("User.NotFound", "No existe el usuario buscado por id");
        public static Error InvalidCredentials = new Error("User.InvalidCredentials", "Las credenciales son incorrectas");
    }
}
