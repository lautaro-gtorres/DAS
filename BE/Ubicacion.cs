using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Ubicacion (catálogo, se carga directamente en la base).
    /// </summary>
    public class Ubicacion
    {
        private int idUbicacion;
        public int IdUbicacion
        {
            get { return idUbicacion; }
            set { idUbicacion = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Lo que se muestra cuando el objeto está en un ComboBox o ListBox.
        public override string ToString()
        {
            return Nombre;
        }
    }
}
