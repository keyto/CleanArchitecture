// https://www.udemy.com/course/clean-architecture-authentication/learn/lecture/41936894#overview
//https://www.udemy.com/course/clean-architecture-authentication/learn/lecture/42032500#questions
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using Dapper;
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

        // esta configurado como Singleton, se puede usar en cualquier parte de la aplicacion
        private readonly ISqlConnectionFactory _sqlConnectionFactory;       

        public JwtProvider(IOptions<JwtOptions> options , ISqlConnectionFactory sqlConnectionFactory)
        {
            _options = options.Value;
            _sqlConnectionFactory = sqlConnectionFactory;
        }


        private async Task<HashSet<string>> GetPermissionsFromIdUser(UserId userId)
        {
            const string sql = """
                
                SELECT p.nombre 
                FROM users usr                 
                LEFT JOIN users_roles usrl ON usr.id = usrl.user_id
                LEFT JOIN roles rl ON rl.id = usrl.user_id
                LEFT JOIN roles_permissions rp ON rp.role_id = rl.id
                LEFT JOIN permissions p ON p.id = rp.permission_id    
                WHERE usr.id = @userId
                """;
            using var connection = _sqlConnectionFactory.CreateConnection();
            var permissions = await connection.QueryAsync<string>(sql, new { @userId = userId.Value });

            var permissionsCollection = permissions.ToHashSet();
            return permissionsCollection;
        }

        /// <summary>
        /// Genera toda la logica para crear el jwt
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> Generate(User user)
        {
            //const string sql = """
            //    SELECT p.nombre 
            //    FROM users usr                 
            //    LEFT JOIN users_roles usrl ON usr.id = usrl.user_id
            //    LEFT JOIN roles rl ON rl.id = usrl.user_id
            //    LEFT JOIN roles_permissions rp ON rp.role_id = rl.id
            //    LEFT JOIN permissions p ON p.id = rp.permission_id    
            //    WHERE usr.id = @userId
            //    """;
            //using var connection = _sqlConnectionFactory.CreateConnection();
            //var permissions = await connection.QueryAsync<string>(sql, new { userId = user.Id!.Value });
            //var permissionsCollection = permissions.ToHashSet();

            var permissionsCollection = await GetPermissionsFromIdUser(user.Id!);

            // crear colecion de claims del Jwt
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
            };

            foreach (var permission in permissionsCollection)
            {                
                  claims.Add(new Claim(CustomClaims.Permissions, permission));
            }
                
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

            return tokenValue;
        }
    }
 
}
