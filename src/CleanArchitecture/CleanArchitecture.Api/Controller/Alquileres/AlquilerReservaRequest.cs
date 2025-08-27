using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Api.Controller.Alquileres
{
    public sealed record AlquilerReservaRequest(         
        Guid VehiculoId,
        Guid UserId,
        DateOnly StartDate,
        DateOnly EndDate
    );
}
