using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler
{
   
    internal sealed class ReservarAlquilerCommandHandler :
        ICommandHandler<ReservarAlquilerCommand, Guid>
    {

        private readonly IUserRepository _userRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IAlquilerRepository _alquilerRepository;
        private readonly PrecioService _precioService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

         

        public ReservarAlquilerCommandHandler(IUserRepository userRepository, IVehiculoRepository vehiculoRepository, IAlquilerRepository alquilerRepository, PrecioService precioService, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _vehiculoRepository = vehiculoRepository;
            _alquilerRepository = alquilerRepository;
            _precioService = precioService;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        

        public async Task<Result<Guid>> Handle(ReservarAlquilerCommand request, CancellationToken cancellationToken)
        {
            // Buscar al usuario
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            // Buscar al vehiculo
            var vehiculo = await _vehiculoRepository.GetByIdAsync(request.VehiculoId, cancellationToken);

            if (vehiculo is null) 
            {             
                return Result.Failure<Guid>(VehiculoErrors.NotFound);
            }

            // Calcular dias a alquilar el vehiculo
            var duracion = DateRange.Create(request.FechaInicio, request.FechaFin);

            // validar si hay OverLaping
            if (await _alquilerRepository.IsOverLappingAsync(vehiculo,duracion,cancellationToken))
            {
                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }

            // se mete en un Try por si hay alguna violacin de bd a la hora de insertar.
            try
            {
                var alquiler = Alquiler.Reservar(vehiculo, user.Id, duracion, _dateTimeProvider.currentTime, _precioService);

                _alquilerRepository.Add(alquiler);

                // persistir en BD
                // SaveChangesAsync coge todo lo que esta en la memoria de Entity FrameWork y lo persiste en BD
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return alquiler.Id;
            }
            catch (ConcurrencyException ex)
            {

                return Result.Failure<Guid>(AlquilerErrors.Overlap);
            }
        }
    }
}
