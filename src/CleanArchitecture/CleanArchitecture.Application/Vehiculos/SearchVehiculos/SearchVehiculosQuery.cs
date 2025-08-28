using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Alquileres.GetAlquiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos
{

    //Funcionalidad : Buscar los vehiculos disponibles en un rango de fechas
    /*
     - Es un record (un DTO inmutable) que representa la petición.
     - Contiene dos parámetros obligatorios:
        fechaInicio
        fechaFin
     - Implementa IQuery<IReadOnlyList<VehiculoResponse>> → significa que al ejecutar esta consulta se espera una lista de VehiculoResponse.

     */
    public sealed record SearchVehiculosQuery(DateOnly fechaInicio, DateOnly fechaFin) 
        : IQuery<IReadOnlyList<VehiculoResponse>>;
}
