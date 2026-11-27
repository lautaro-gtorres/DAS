using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Composite: la hoja. Un corral con sus propios datos (no tiene hijos).
    /// </summary>
    public class CorralGranja : ComponenteGranja
    {
        public CorralGranja(string nroCorral, int gallinas, int capacidad, int huevosUltimoMes)
            : base("Corral " + nroCorral)
        {
            this.gallinas = gallinas;
            this.capacidad = capacidad;
            this.huevosUltimoMes = huevosUltimoMes;
        }

        private int gallinas;
        private int capacidad;
        private int huevosUltimoMes;

        public override string Tipo()
        {
            return "Corral";
        }

        public override int Gallinas()
        {
            return gallinas;
        }

        public override int Capacidad()
        {
            return capacidad;
        }

        public override int HuevosUltimoMes()
        {
            return huevosUltimoMes;
        }
    }
}
