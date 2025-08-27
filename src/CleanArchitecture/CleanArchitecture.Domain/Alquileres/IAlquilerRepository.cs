using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Alquileres
{
    public interface IAlquilerRepository
    {
        Task<Alquiler?>GetByIdAsync(Guid id,CancellationToken cancellationToken= default);

        Task<bool> IsOverLappingAsync(Vehiculo vehiculo, DateRange duracion, CancellationToken cancellation = default);

        void Add(Alquiler alquiler);
    }
}
