using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Patrón Composite: la estructura de la granja como árbol.
    ///   Granja (GrupoGranja) → Ubicación (GrupoGranja) → Corral (CorralGranja, la hoja).
    /// Cualquier nodo responde lo mismo (gallinas, capacidad, huevos) sin que quien lo usa
    /// sepa si es un corral o un grupo: el corral devuelve sus datos y el grupo suma los de sus hijos.
    /// </summary>
    public abstract class ComponenteGranja
    {
        protected ComponenteGranja(string nombre)
        {
            this.nombre = nombre;
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // "Granja", "Ubicación" o "Corral".
        public abstract string Tipo();

        // Gallinas activas.
        public abstract int Gallinas();

        // Lugares (capacidad máxima). Un corral sin capacidad cargada cuenta 0.
        public abstract int Capacidad();

        // Huevos producidos en el último mes.
        public abstract int HuevosUltimoMes();

        // Los nodos que cuelgan de este. Una hoja no tiene hijos.
        public virtual List<ComponenteGranja> Hijos()
        {
            return new List<ComponenteGranja>();
        }

        // Lugares que quedan; se calcula igual para hojas y grupos.
        public int LugaresLibres()
        {
            int libres = Capacidad() - Gallinas();
            if (libres < 0)
                return 0;
            return libres;
        }

        // Porcentaje de ocupación (0 si no tiene capacidad cargada).
        public int Ocupacion()
        {
            if (Capacidad() == 0)
                return 0;
            return Gallinas() * 100 / Capacidad();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
