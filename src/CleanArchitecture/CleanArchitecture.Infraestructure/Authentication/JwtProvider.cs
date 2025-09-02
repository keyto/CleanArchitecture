// https://www.udemy.com/course/clean-architecture-authentication/learn/lecture/41936894#overview
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        // para trabajar con los valores del appsetting
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public Task<string> Generate(User user)
        {
            // crear colecion de claims
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
                SecurityAlgorithms.HmacSha256    
            );

            // creacion del token
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
            );

            // obtener el token value
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            // como no hay ningun metodo await en este metodo
            // tengo que devolverlo de esta manera, si no seria un return tokenValue
            return Task.FromResult<string>(tokenValue);

        }
    }
}
