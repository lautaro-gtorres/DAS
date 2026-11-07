using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla TipoProducto (catálogo): Huevo, Pollo.
    /// </summary>
    public class TipoProducto
    {
        private int idTipoProducto;
        public int IdTipoProducto
        {
            get { return idTipoProducto; }
            set { idTipoProducto = value; }
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
