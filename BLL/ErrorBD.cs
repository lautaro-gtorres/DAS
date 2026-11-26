using System;
using System.Data.SqlClient;

namespace BLL
{
    /// <summary>
    /// Traduce los errores de la base que se pueden explicar al usuario
    /// (dato repetido o registro en uso) a ArgumentException con un mensaje claro.
    /// El resto de los errores se relanzan tal cual.
    /// </summary>
    internal static class ErrorBD
    {
        const int ClaveUnica = 2627;        // UNIQUE / PRIMARY KEY
        const int IndiceUnico = 2601;       // índice UNIQUE
        const int ClaveForanea = 547;       // FOREIGN KEY o CHECK

        public static int Ejecutar(Func<int> accion, string mensajeRepetido = null, string mensajeEnUso = null)
        {
            try
            {
                return accion();
            }
            catch (SqlException ex) when (mensajeRepetido != null && (ex.Number == ClaveUnica || ex.Number == IndiceUnico))
            {
                throw new ArgumentException(mensajeRepetido, ex);
            }
            catch (SqlException ex) when (mensajeEnUso != null && ex.Number == ClaveForanea)
            {
                throw new ArgumentException(mensajeEnUso, ex);
            }
        }
    }
}
