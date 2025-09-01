using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Reviews.Events
{
    public sealed record ReviewCreateDomainEvent(ReviewId reviewId):IDomainEvent;
}
