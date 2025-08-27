/*video 34*/
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

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos
{
    internal sealed class SearchVehiculosQueryHandler 
       : IQueryHandler<SearchVehiculosQuery, IReadOnlyList<VehiculoResponse>>
    {
        private static readonly int[] ActiveAlquilerStatuses = 
        {
           (int)AlquilerStatus.Reservado,
           (int)AlquilerStatus.Confirmado,
           (int)AlquilerStatus.Completado
        };
        private readonly IPostgresConnectionFactory _PostgresConnectionFactory;

        public SearchVehiculosQueryHandler(IPostgresConnectionFactory postgresConnectionFactory)
        {
            _PostgresConnectionFactory = postgresConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<VehiculoResponse>>> Handle(SearchVehiculosQuery request, CancellationToken cancellationToken)
        {
            // validacion de fechas 
            if (request.fechaInicio > request.fechaFin) {
                return new List<VehiculoResponse>();
            }

            using var connection = _PostgresConnectionFactory.CreateConnection();

            const string sql = """
                 SELECT 
                    a.id as Id
                    ,a.modelo as Modelo
                    ,a.vin as Vin
                    ,a.precio_monto as Precio
                    ,a.precio_tipo_moneda as TipoMoneda
                    ,a.direccion_pais as Pais
                    ,a.direccion_departamento as Departamento
                    ,a.direccion_provincia  as  Provincia
                    ,a.direccion_ciudad as  Ciudad
                    ,a.direccion_calle as  Calle
                  
                 FROM vehiculos AS a
                 WHERE NOT EXISTS 
                 (
                    SELECT 1 
                    FROM alquileres as b
                    WHERE a.id  = b.vehiculo_id
                    AND b.duracion_inicio <= @EndDate 
                    AND b.duracion_fin <= @StartDate 
                    AND b.status = ANY(@activeAlquilerStatuses)
                 )                
                """;

            //En el sql haydatos para rellenar el objeto  VehiculoResponse y el objeto DireccionResponse

            var vehiculos = await connection
                // Hacer query y maperar los valores en objetos de tipo :
                // 1er parametro de QueryAsync :  VehiculoResponse
                // 2do parametro de QueryAsync :  DireccionResponse
                // 3er parametro de QueryAsync :  VehiculoResponse que es la clase que contiene esos 2 objetosya que DireccionResponse es parte de VehiculoResponse
                .QueryAsync<VehiculoResponse, DireccionResponse, VehiculoResponse>
                (
                    sql,
                    //Esto se hace para separar los valores de la query y se carguen en VehiculoResponse o DireccionResponse
                    (vehiculo, direccion) =>
                    {
                        vehiculo.Direccion = direccion;
                        return vehiculo;
                    },
                    // parametros
                    new
                    {
                        StartDate = request.fechaInicio,
                        EndDate = request.fechaFin,
                        activeAlquilerStatuses = ActiveAlquilerStatuses
                    },
                    // Desde la columna Pais del Select, lo mete en el objeto DireccionResponse
                    splitOn: "Pais"

                );
            
            return vehiculos.ToList();
        }
    }
}
