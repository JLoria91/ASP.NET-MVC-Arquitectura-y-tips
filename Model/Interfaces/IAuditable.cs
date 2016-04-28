using System;

namespace Model.Interfaces
{
    public interface IAuditable
    {
        int CreadoPor { get; set; }
        DateTime CreadoFecha { get; set; }
        int ActualizadoPor { get; set; }
        DateTime ActualizadoFecha { get; set; }
    }
}
