using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Pasajero
    {
        private int idPasajero;
        public int IdPasajero
        {
            get { return idPasajero; }
            set { idPasajero = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }

        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        // Lo que muestra el ComboBox
        public override string ToString()
        {
            return apellido + ", " + nombre;
        }
    }
}
