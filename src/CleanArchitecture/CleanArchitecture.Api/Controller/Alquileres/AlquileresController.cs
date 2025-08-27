using CleanArchitecture.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Application.Alquileres.ReservarAlquiler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controller.Alquileres
{
    [ApiController]
    [Route("api/alquileres")]
    public class AlquileresController : ControllerBase
    {
        private readonly ISender _sender;  // objeto de Mediatr

        public AlquileresController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlquiler(Guid Id,CancellationToken cancellation) 
        {
            var query = new GetAlquilerQuery(Id);

            var resultado = await _sender.Send(query,cancellation);
            
            return resultado.IsSuccess? Ok(resultado.Value) : NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ReservarAlquiler(Guid Id, AlquilerReservaRequest request, CancellationToken cancellation)
        {
            var command = new ReservarAlquilerCommand(request.VehiculoId, request.UserId,request.StartDate,request.EndDate);

            var resultado = await _sender.Send(command, cancellation);

            if (resultado.IsFailure)
            {
                return BadRequest(resultado.Error);
            }

            // Llamamos al metodo GetAlquiler de este controlador, pasandole como parametro el Id Alquiler
            return CreatedAtAction(nameof(GetAlquiler),new {id = resultado.Value });

        }

    }
}
