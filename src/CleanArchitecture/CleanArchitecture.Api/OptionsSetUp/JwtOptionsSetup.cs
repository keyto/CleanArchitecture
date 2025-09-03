using CleanArchitecture.Infraestructure.Authentication;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Api.OptionsSetUp
{

    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            // leer toda la seccion del appsetting llamada "Jwt" con GetSection
            // y meterla en un objeto de tipo JwtOptions .
            // Se mapean las propiedades automaticamente
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}

 