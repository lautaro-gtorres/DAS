using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Produccion: huevos recolectados por corral y por día (uno por corral y fecha).
    /// </summary>
    public class Produccion
    {
        private int idProduccion;
        public int IdProduccion
        {
            get { return idProduccion; }
            set { idProduccion = value; }
        }

        private int idCorral;
        public int IdCorral
        {
            get { return idCorral; }
            set { idCorral = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        private string nroCorral;
        public string NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private int cantidadHuevos;
        public int CantidadHuevos
        {
            get { return cantidadHuevos; }
            set { cantidadHuevos = value; }
        }
    }
}
