using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application;
using CleanArchitecture.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApplication();  // para añadir el contenedor de dependencias de CleanArchitecture.Application
builder.Services.AddInfraestructure(builder.Configuration);  // para añadir el contenedor de dependencias de CleanArchitecture.Infraestructure

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//https://www.udemy.com/course/clean-architecture/learn/lecture/40759798#overview
//Ejecutar la migracion de las tablas
// Desde la terminal de VsCode ejecutar el comando : esto crea el directorio CleanArchitecture.Infraestructure/Migrations
// dotnet ef --verbose migrations add InitialCreate -p src/CleanArchitecture/CleanArchitecture.Infraestructure -s src/CleanArchitecture/CleanArchitecture.Api
// Al crear ese comando solo se crean los archivos de migracion, pero no se crean las tablas en la bd
// Al ejecutar el proyecto, ya se crean las tablas .
// Ejecutamos el comando : dotnet run --project src/CleanArchitecture/CleanArchitecture.Api
app.ApplyMigration();

// llamamos al metodo de extension para que cree los registros de prueba
app.SeedData();

// Inyecto la gestion de excepciones del Middleware
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();
 
