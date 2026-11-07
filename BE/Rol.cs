using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Rol (catálogo): Administrador, Operario.
    /// </summary>
    public class Rol
    {
        private int idRol;
        public int IdRol
        {
            get { return idRol; }
            set { idRol = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        // Lo que se muestra cuando el objeto está en un ComboBox o ListBox.
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
