using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Gallina
    {
        private int nroGallina;
        public int NroGallina
        {
            get { return nroGallina; }
            set { nroGallina = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string raza;
        public string Raza
        {
            get { return raza; }
            set { raza = value; }
        }

        private DateTime fechaNacimiento;
        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private int nroCorral;
        public int NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }
    }
}
