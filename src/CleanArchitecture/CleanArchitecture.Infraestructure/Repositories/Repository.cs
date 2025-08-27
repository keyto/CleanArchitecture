using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Repositories
{
    // Implementacion de los metodos genericos que tienen todas las entidades,
    // Si alguna entidad tiene algun otro metodo especifico habria que implementarlo en su interfaz en el directorio 
    // // de CleanArchitecture.Domain.XXXX 
    internal abstract class Repository<T>
        where T : Entity
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken) 
        {
            return await DbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
        }

        public void Add(T Entity)
        {
            DbContext.Add(Entity);
        }
    }
}
