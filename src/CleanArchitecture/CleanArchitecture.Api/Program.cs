using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.OptionsSetUp;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infraestructure;
using CleanArchitecture.Infraestructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//registra controladores para la Web API.
builder.Services.AddControllers();

// Authentication : Se indica que mi app tiene un proceso de autenticacion que se basa en el jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();  
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddTransient<IJwtProvider,JwtProvider>();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


// activar Swagger (documentaci�n/try-it).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// registra casos de uso, MediatR, validaciones, etc. de tu capa Application.
builder.Services.AddApplication();  // para a�adir el contenedor de dependencias de CleanArchitecture.Application

//registra DbContext, repositorios, conexiones, etc. (capa Infrastructure) usando la configuraci�n (appsettings.json).
builder.Services.AddInfraestructure(builder.Configuration);  // para a�adir el contenedor de dependencias de CleanArchitecture.Infraestructure

//construye la app con todo lo registrado.
var app = builder.Build();

// Si el entorno es Development, habilita Swagger y su UI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    //https://www.udemy.com/course/clean-architecture/learn/lecture/40759798#overview
    //Ejecutar la migracion de las tablas
    // Desde la terminal de VsCode ejecutar el comando : esto crea el directorio CleanArchitecture.Infraestructure/Migrations
    // dotnet ef --verbose migrations add InitialCreate -p src/CleanArchitecture/CleanArchitecture.Infraestructure -s src/CleanArchitecture/CleanArchitecture.Api
    // Al crear ese comando solo se crean los archivos de migracion, pero no se crean las tablas en la bd
    // Al ejecutar el proyecto, ya se crean las tablas .
    // Ejecutamos el comando : dotnet run --project src/CleanArchitecture/CleanArchitecture.Api
    await app.ApplyMigration();

    // llamamos al metodo de extension para que cree los registros de prueba
    app.SeedData();

    app.SeedDataAuthentication();
}

// Inyecto la gestion de excepciones del Middleware
app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

//Mapea los controladores a rutas HTTP.
app.MapControllers();

//Arranca el servidor Kestrel y se queda escuchando.
app.Run();
 
