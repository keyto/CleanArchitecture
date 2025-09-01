using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Repositories
{
    internal sealed class AlquilerRepository : Repository<Alquiler,AlquilerId>, IAlquilerRepository
    {
        private static readonly AlquilerStatus[] ActiveAlquilerStatuses = { 
            AlquilerStatus.Reservado,
            AlquilerStatus.Confirmado,
            AlquilerStatus.Completado
        }; 
        public AlquilerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsOverLappingAsync(Vehiculo vehiculo, DateRange duracion, CancellationToken cancellationToken = default)
        {
            // Devolver True o False.
            // True si en esas fechas esta alquilado el vehiculo
            // False si no esta alquilado y esta disponible para alquilarlo
            return await DbContext.Set<Alquiler>()
                .AnyAsync(
                    alquiler =>
                        alquiler.VehiculoId == vehiculo.Id &&
                        alquiler.Duracion.Inicio<= duracion.Fin &&
                        alquiler.Duracion.Fin >= duracion.Inicio &&
                        ActiveAlquilerStatuses.Contains(alquiler.Status),
                        cancellationToken
                );
        }
    }
}
