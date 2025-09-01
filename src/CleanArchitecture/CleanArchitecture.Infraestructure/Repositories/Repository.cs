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
    // de CleanArchitecture.Domain.XXXX 
    internal abstract class Repository<TEntity, TEntityId>
        where TEntity : Entity<TEntityId>
        where TEntityId : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<TEntity?> GetByIdAsync(TEntityId Id, CancellationToken cancellationToken) 
        {
            return await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
        }

        public void Add(TEntity Entity)
        {
            DbContext.Add(Entity);
        }
    }
}
