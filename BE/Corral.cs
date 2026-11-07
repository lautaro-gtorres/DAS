using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Corral.
    /// </summary>
    public class Corral
    {
        private int idCorral;
        public int IdCorral
        {
            get { return idCorral; }
            set { idCorral = value; }
        }

        // Número visible del corral (ej: "C-01"). Es único.
        private string nroCorral;
        public string NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }

        private int idTipoCorral;
        public int IdTipoCorral
        {
            get { return idTipoCorral; }
            set { idTipoCorral = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        // Ponedoras, Engorde o Cria.
        private string tipoCorral;
        public string TipoCorral
        {
            get { return tipoCorral; }
            set { tipoCorral = value; }
        }

        // Opcional en la base.
        private int? capacidad;
        public int? Capacidad
        {
            get { return capacidad; }
            set { capacidad = value; }
        }

        // Lo que se muestra cuando el objeto está en un ComboBox o ListBox.
        public override string ToString()
        {
            return NroCorral;
        }
    }
}
