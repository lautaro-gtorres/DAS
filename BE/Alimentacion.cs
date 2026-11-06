using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Alimentacion
    {
        private int nroAlimentacion;
        public int NroAlimentacion
        {
            get { return nroAlimentacion; }
            set { nroAlimentacion = value; }
        }

        private int nroAlimento;
        public int NroAlimento
        {
            get { return nroAlimento; }
            set { nroAlimento = value; }
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

        private decimal cantidad;
        public decimal Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }
    }
}
