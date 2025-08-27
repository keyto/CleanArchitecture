//https://www.udemy.com/course/clean-architecture/learn/lecture/40759798#overview

using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CleanArchitecture.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        // Metodo de extension para poder usarlo en la clase program.cs
        public static async void ApplyMigration(this IApplicationBuilder app)
        {
            // cree un contexto
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider;
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();   // crear un obj logerFactory

                try
                {
                    var context = service.GetRequiredService<ApplicationDbContext>();
                    // Ejecuta en la bd de Postgree los archivos de migracion creados en el proyecto,
                    // los de l folder Migrations
                    await context.Database.MigrateAsync();

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex,"Error en migracion");
                }

            }
        
        }


        public static void UseCustomExceptionHandler(this IApplicationBuilder app) 
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
