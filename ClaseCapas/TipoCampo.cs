using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseCapas
{
    // Qué tipo de dato acepta un CampoValidado (se elige en el diseñador, propiedad TipoDato).
    public enum TipoCampo
    {
        Texto,
        Entero,
        Decimal,
        Email,
        Dni,
        Telefono
    }
}