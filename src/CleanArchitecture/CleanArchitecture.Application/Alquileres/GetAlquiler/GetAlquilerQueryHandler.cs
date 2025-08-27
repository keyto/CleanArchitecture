/*clase 32 : https://www.udemy.com/course/clean-architecture/learn/lecture/40747570#overview*/

using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Alquileres.GetAlquiler
{
    // es internal porque no necesita ser expuesto a los controller.
    // Lo que es expuesto es el Query (El que le llama)
    internal sealed class GetAlquilerQueryHandler : IQueryHandler<GetAlquilerQuery, AlquilerResponse>
    {
        private readonly ISqlConnectionFactory _SqlConnectionFactory;

        public GetAlquilerQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _SqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<AlquilerResponse>> Handle(GetAlquilerQuery request, CancellationToken cancellationToken)
        {
            using var _connection =  _SqlConnectionFactory.CreateConnection();

            var sql = """
                SELECT 
                    id AS Id,
                    vehiculo_id AS VehiculoId,
                    user_id AS UserId,
                    status AS Status,
                    precio_por_periodo AS PrecioAlquiler,
                    precio_por_periodo_tipo_moneda AS TipoMonedaAlquiler,
                    precio_mantenimiento AS PrecioMantenimiento,
                    precio_mantenimiento_tipo_moneda AS TipoMonedaMantenimiento,
                    precio_accesorios AS AccesoriosPrecio,
                    precio_accesorios_tipo_moneda AS TipoMonedaAccesorio,
                    precio_total AS PrecioTotal,
                    precio_total_tipo_moneda AS PrecioTotalTipoMoneda,
                    duracion_inicio AS DuracionInicio,
                    duracion_final AS DuracionFinal,
                    fecha_creacion AS FechaCreacion
                FROM alquileres
                WHERE id = @AlquilerId 
                """;


            // Como solo quiero que me devuelva 1 registro. se llama al metodo QueryFirstOrDefaultAsync
            // y el resultado se va a mapear con el objeto AlquilerResponse            
            var alquiler = await _connection.QueryFirstOrDefaultAsync<AlquilerResponse>(
                sql,
                new { 
                    request.AlquilerId
                }
            );

            return alquiler;
        }
    }
}
