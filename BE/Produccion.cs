using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Produccion
    {
        private int nroProduccion;
        public int NroProduccion
        {
            get { return nroProduccion; }
            set { nroProduccion = value; }
        }

        private int nroCorral;
        public int NroCorral
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

        private string observaciones;
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
    }
}
