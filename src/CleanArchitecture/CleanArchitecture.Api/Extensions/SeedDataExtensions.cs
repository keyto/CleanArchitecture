//https://www.udemy.com/course/clean-architecture/learn/lecture/40759808#overview
// Queremos crear datos de prueba en las tablas .
// Se instala el paquete de Nuget Bogus

using Bogus;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure;
using Dapper;
using Microsoft.OpenApi.Writers;
using System.Threading.Tasks;

namespace CleanArchitecture.Api.Extensions
{
    public static class SeedDataExtensions
    {
        // crear usuarios de prueba
        public static void SeedDataAuthentication(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope() ;
            var service = scope.ServiceProvider;

            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();

                // si no hay usuarios creados
                if (!context.Set<User>().Any())
                {
                    // inserta un nuevo usuario
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("password1&");
                    var user = User.Create(
                        new Nombre("Fernando")
                        ,new Apellido("Jimenez")
                        ,new Email("keitoo@hotmail.com")
                        ,new PasswordHash(passwordHash)
                        );

                    context.Add(user);

                    // inserta un nuevo usuario
                    var passwordHash2 = BCrypt.Net.BCrypt.HashPassword("password2&");
                    var user2 = User.Create(
                        new Nombre("Maria")
                        , new Apellido("Quintela")
                        , new Email("MariaQuintela@hotmail.com")
                        , new PasswordHash(passwordHash2)
                        );

                    context.Add(user);


                    // si el metodo  no es asincrono se usa esto.
                    context.SaveChangesAsync().Wait();
                }


            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex.Message);
            }
        }

        public static void SeedData(this IApplicationBuilder app)
        {
            // utilizamos Dapper para insertar registros
            // creamos un scope      
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider;
                var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

                // creamos conexion a la bd
                using var connection = sqlConnectionFactory.CreateConnection();

                //instanciar el obj que nos permite crear los datos de prueba
                var faker = new Faker();
                List<object> vehiculos = new();
                for (int i = 0; i < 100; i++)
                {
                    vehiculos.Add(new
                    {
                        Id = Guid.NewGuid(),
                        Vin = faker.Vehicle.Vin(),
                        Modelo = faker.Vehicle.Model(),
                        Pais = faker.Address.Country(),
                        Departamento = faker.Address.State(),
                        Provincia = faker.Address.County(),
                        Ciudad = faker.Address.City(),
                        Calle = faker.Address.StreetAddress(),
                        PrecioMonto = faker.Random.Decimal(1000, 10000),
                        PrecioTipoMoneda = "USD",
                        PrecioMantenimiento = faker.Random.Decimal(100, 200),
                        PrecioMantenimientoTipoMoneda = "USD",
                        Accesorios = new List<int> { (int)Accesorio.Wifi, (int)Accesorio.AppleCar },
                        FechaUltima = DateTime.MinValue
                    });
                }

                const string sql = """
                    INSERT INTO public.vehiculos
                    (id,vin,modelo,direccion_pais,direccion_departamento,direccion_provincia,direccion_ciudad,direccion_calle,precio_monto,precio_tipo_moneda,mantenimiento_monto,mantenimiento_tipo_moneda,accesorios,fecha_ultima_alquiler)
                    values
                    (@Id,@Vin,@modelo,@Pais,@Departamento,@Provincia,@Ciudad,@Calle,@PrecioMonto,@PrecioTipoMoneda,@PrecioMantenimiento,@PrecioMantenimientoTipoMoneda,@Accesorios,@FechaUltima);                    
                    """;

                // Ejecutar el query
                connection.Execute(sql, vehiculos);

            }
        }
    }
}
