using CleanArchitecture.Application.Users.LoginUsers;
using CleanArchitecture.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controller.Users
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender; // objeto de Mediatr

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request,CancellationToken cancellationToken)
        {
            var query = new LoginCommand(request.Login, request.Password);

            var resultados = await _sender.Send(query, cancellationToken);

            if (resultados.IsFailure)
            {
                return Unauthorized(resultados.Error);
            }

            // aqui se devuelve el valor del token
            return Ok(resultados.Value);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            // crear el command
            var query = new RegisterUserCommand( 
                request.Email,
                request.Nombre,
                request.Apellidos, 
                request.Password
            );

            // llamar al handler
            var resultados = await _sender.Send(query, cancellationToken);

            if (resultados.IsFailure)
            {
                return Unauthorized(resultados.Error);
            }

            // aqui se devuelve el valor del token
            return Ok(resultados.Value);
        }
    }
}
