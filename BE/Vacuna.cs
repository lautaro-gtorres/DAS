using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Vacuna
    {
        private int nroVacuna;
        private string nombre;
        private string descripcion;
        private string laboratorio;
        private int stock;

        public int NroVacuna
        {
            get { return nroVacuna; }
            set { nroVacuna = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string Laboratorio
        {
            get { return laboratorio; }
            set { laboratorio = value; }
        }

        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }
    }
}
