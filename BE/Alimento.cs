using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Alimento
    {
        private int nroAlimento;
        public int NroAlimento
        {
            get { return nroAlimento; }
            set { nroAlimento = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private string unidadMedida;
        public string UnidadMedida
        {
            get { return unidadMedida; }
            set { unidadMedida = value; }
        }

        private decimal stock;
        public decimal Stock
        {
            get { return stock; }
            set { stock = value; }
        }

    }
}
