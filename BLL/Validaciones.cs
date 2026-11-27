using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Validaciones que usan varias BLL (para no repetir el mismo código en cada una).
    internal static class Validaciones
    {
        // Devuelve el texto sin espacios de los costados, o null si está vacío.
        public static string SacarEspacios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;
            return texto.Trim();
        }

        // Control simple de e-mail: algo antes de la @ y un punto después.
        public static bool EsEmailValido(string email)
        {
            int arroba = email.IndexOf('@');
            if (arroba < 1)
                return false;
            if (email.LastIndexOf('.') < arroba + 2)
                return false;
            if (email.EndsWith("."))
                return false;
            return true;
        }
    }
}
