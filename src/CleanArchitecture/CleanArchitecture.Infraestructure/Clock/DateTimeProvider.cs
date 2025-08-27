using CleanArchitecture.Application.Abstractions.Clock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Clock
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime currentTime => DateTime.UtcNow; // Devolver la fecha hora actual
    }
}
