using CleanArchitecture.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CleanArchitecture.Application.Users.LoginUsers
{
    // ICommand<string> indica que hereda de Icommand y que devolverá un string (el token)
    public record LoginCommand (string Email,string Password) : ICommand<string>;  
}
