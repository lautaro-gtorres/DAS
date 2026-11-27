using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Composite: el compuesto. La granja o una ubicación (galpón, campo).
    /// Tiene una lista de hijos (corrales u otros grupos) y cada dato es la suma de sus hijos.
    /// </summary>
    public class GrupoGranja : ComponenteGranja
    {
        public GrupoGranja(string nombre, string tipo) : base(nombre)
        {
            this.tipo = tipo;
        }

        private string tipo;

        // Lista polimórfica: puede tener corrales (hojas) y otros grupos.
        private List<ComponenteGranja> hijos = new List<ComponenteGranja>();

        public void Agregar(ComponenteGranja hijo)
        {
            hijos.Add(hijo);
        }

        public override List<ComponenteGranja> Hijos()
        {
            return hijos;
        }

        public override string Tipo()
        {
            return tipo;
        }

        // Cada hijo responde lo suyo: si es otro grupo, a su vez suma a sus hijos (recursivo).
        public override int Gallinas()
        {
            int total = 0;
            foreach (ComponenteGranja hijo in hijos)
                total = total + hijo.Gallinas();
            return total;
        }

        public override int Capacidad()
        {
            int total = 0;
            foreach (ComponenteGranja hijo in hijos)
                total = total + hijo.Capacidad();
            return total;
        }

        public override int HuevosUltimoMes()
        {
            int total = 0;
            foreach (ComponenteGranja hijo in hijos)
                total = total + hijo.HuevosUltimoMes();
            return total;
        }
    }
}
