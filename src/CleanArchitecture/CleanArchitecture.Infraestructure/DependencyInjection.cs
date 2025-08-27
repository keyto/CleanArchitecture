using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure.Clock;
using CleanArchitecture.Infraestructure.Data;
using CleanArchitecture.Infraestructure.Email;
using CleanArchitecture.Infraestructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; /*Microsoft.Extensions.Configuration.Abstraction*/
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure
{
    public static class DependencyInjection
    {

        /*Se  instala el Nuget Microsoft.Extensions.Configuration.Abstraction para poder usar el IConfiguration*/
        public static IServiceCollection AddInfraestructure(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            // configuration tiene acceso a los valores del archivo appsetting.json del API
            var connectionString = configuration.GetConnectionString("Database")
                ?? throw new ArgumentException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAlquilerRepository, AlquilerRepository>();
            services.AddScoped<IVehiculoRepository, VehiculoRepository>();

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();

            // registrar el mapeador que se creo en CleanArchitecture.Infraestructure.Data.DateOnlyTypeHandler
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            // inyectar la cadena de conexiopm para el IPostgresConnectionFactory
            services.AddSingleton<IPostgresConnectionFactory>(_ => new PostgresConnectionFactory(connectionString) );


            // inyectar la cadena de conexion para SQL Server
            var connectionStringSql = configuration.GetConnectionString("DatabaseSql")
               ?? throw new ArgumentException(nameof(configuration));

            services.AddSingleton<ISqlServerConnectionFactory>(_ => new SqlServerConnectionFactorySql(connectionStringSql));

            return services;
        }
    }
}
