using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.LoginUsers
{
    //  ICommandHandler<LoginCommand,string> : Significa que hereda de ICommandHandler,
    //                                          que espera recibir un LoginCommand
    //                                          y que devolvera un string
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand,string>
    {
    
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

      

        public async Task<Result<string>>Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1 : Verificar que exista el user por email
            var  user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);
            if (user is null)
            {
                return Result.Failure<string>(UserErrors.NotFound);
            }

            
            // 2 : Validar el password recibido (texto plano) con el que esta guardado en la bd (encriptado) 
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash!.Value))
            {
                return Result.Failure<string>(UserErrors.InvalidCredentials);
            }

            // 3 : Generar el jwt con los Claims del usuario.
            var token = await _jwtProvider.Generate(user);


            // 4 devolver el jwt
            // return jwt
            return token;
        }
    }
}
