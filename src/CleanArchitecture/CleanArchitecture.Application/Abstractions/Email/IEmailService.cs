using CleanArchitecture.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendAsync(CleanArchitecture.Domain.Users.Email recipient, string subject, string body);
    }
}
