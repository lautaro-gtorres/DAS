using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Raza (catálogo).
    /// </summary>
    public class Raza
    {
        private int idRaza;
        public int IdRaza
        {
            get { return idRaza; }
            set { idRaza = value; }
        }

        private string nombreRaza;
        public string NombreRaza
        {
            get { return nombreRaza; }
            set { nombreRaza = value; }
        }

        // Lo que se muestra cuando el objeto está en un ComboBox o ListBox.
        public override string ToString()
        {
            return NombreRaza;
        }
    }
}
