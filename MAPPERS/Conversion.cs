using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    // Conversiones entre los valores opcionales de C# (null) y los de la base (DBNull).
    // Las usan varios mappers, así que están en un solo lugar.
    internal static class Conversion
    {
        // Para los parámetros: null se manda como NULL a la base.
        public static object TextoONulo(string texto)
        {
            if (texto == null)
                return DBNull.Value;
            return texto;
        }

        public static object NumeroONulo(int? numero)
        {
            if (numero == null)
                return DBNull.Value;
            return numero.Value;
        }

        // Para leer filas: NULL de la base queda en null.
        public static string TextoDeFila(DataRow row, string columna)
        {
            if (row[columna] == DBNull.Value)
                return null;
            return row[columna].ToString();
        }

        public static int? NumeroDeFila(DataRow row, string columna)
        {
            if (row[columna] == DBNull.Value)
                return null;
            return Convert.ToInt32(row[columna]);
        }
    }
}
