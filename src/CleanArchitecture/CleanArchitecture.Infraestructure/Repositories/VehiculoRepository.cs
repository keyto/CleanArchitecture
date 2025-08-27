using CleanArchitecture.Domain.Vehiculos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Repositories
{
    internal sealed class VehiculoRepository : Repository<Vehiculo>, IVehiculoRepository
    {
        public VehiculoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
