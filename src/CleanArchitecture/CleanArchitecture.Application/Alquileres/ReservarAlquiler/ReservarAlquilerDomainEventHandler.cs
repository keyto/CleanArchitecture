using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler
{
    //  INotificationHandler<AlquilerReservadoDomainEvent> Especifica que esta clase esta escuchando las notificaciones
    //  // del evento AlquilerReservadoDomainEvent
    internal sealed class ReservarAlquilerDomainEventHandler : INotificationHandler<AlquilerReservadoDomainEvent>
    {

        private readonly IAlquilerRepository _alquilerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        public ReservarAlquilerDomainEventHandler(IAlquilerRepository alquilerRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _alquilerRepository = alquilerRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(AlquilerReservadoDomainEvent notification, CancellationToken cancellationToken)
        {
            // escribir la logica que se dispara al crear el evento de Reserva de Alquileres "AlquilerReservadoDomainEvent"
            var alquiler = await _alquilerRepository.GetByIdAsync(notification.AlquilerId, cancellationToken);
            if (alquiler == null) {
                return;
            }

            var user = await _userRepository.GetByIdAsync(alquiler.UserId!,cancellationToken);
            if (user == null) {
                return;
            }

            await _emailService.SendAsync(user.Email!, "Alquiler Reservado", "Tienes que confirmar la reserva para continuar.");
        }
    }
}
