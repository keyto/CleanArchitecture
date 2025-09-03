using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Repositories
{
    internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email); 
        }

        public async Task<bool> IsUserExists(Domain.Users.Email email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<User>()
                .AnyAsync(x => x.Email == email);
        }
    }
}