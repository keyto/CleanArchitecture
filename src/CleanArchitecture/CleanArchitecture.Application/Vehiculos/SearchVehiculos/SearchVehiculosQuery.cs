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
     public sealed record SearchVehiculosQuery(DateOnly fechaInicio, DateOnly fechaFin) 
        : IQuery<IReadOnlyList<VehiculoResponse>>;
}
