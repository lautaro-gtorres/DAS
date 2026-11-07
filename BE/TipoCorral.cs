using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla TipoCorral (catálogo): Ponedoras, Engorde, Cria.
    /// </summary>
    public class TipoCorral
    {
        private int idTipoCorral;
        public int IdTipoCorral
        {
            get { return idTipoCorral; }
            set { idTipoCorral = value; }
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
