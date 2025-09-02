using CleanArchitecture.Application.Users.LoginUsers;
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
        [HttpGet]
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
    }
}
