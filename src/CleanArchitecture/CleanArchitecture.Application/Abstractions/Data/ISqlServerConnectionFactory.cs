using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Data
{     
    public interface ISqlServerConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
