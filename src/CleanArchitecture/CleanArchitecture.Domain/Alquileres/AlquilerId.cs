using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Alquileres
{ 
    public record AlquilerId(Guid Value)
    {
        public static AlquilerId New() => new(Guid.NewGuid());
    }
}
