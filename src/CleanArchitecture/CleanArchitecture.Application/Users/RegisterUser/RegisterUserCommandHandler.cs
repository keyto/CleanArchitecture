//https://www.udemy.com/course/clean-architecture-authentication/learn/lecture/41965244#overview
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.RegisterUser
{
    internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
                RegisterUserCommand request, 
                CancellationToken cancellationToken)
        {
            // 1. Validar que no exista el usuario.          
            if (await _userRepository.IsUserExists(new Email(request.Email), cancellationToken))
            {
                return Result.Failure<Guid>(UserErrors.AlreadyExists);
            }

            // 2. Encriptar el password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Crear un objeto de tipo user 
            var user = User.Create(
                new Nombre(request.Nombre), 
                new Apellido(request.Apellidos), 
                new Email(request.Email), 
                new PasswordHash(passwordHash)
                );

           // 4.Insertar el user en la memoria del EF. NO EN LA BD!!!
           _userRepository.Add(user);

            // Guarda en la Bd todos los cambios del EF
            await _unitOfWork.SaveChangesAsync();

            return user.Id!.Value;
        }
    }
}
