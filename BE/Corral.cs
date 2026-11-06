using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Corral
    {
        private int nroCorral;
        public int NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private int capacidad;
        public int Capacidad
        {
            get { return capacidad; }
            set { capacidad = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private bool estado;
        public bool Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}
