using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla TipoGallina (catálogo): Ponedora, Engorde, Gallo.
    /// </summary>
    public class TipoGallina
    {
        private int idTipoGallina;
        public int IdTipoGallina
        {
            get { return idTipoGallina; }
            set { idTipoGallina = value; }
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
