using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Vacunacion
    {
        private int nroVacunacion;
        public int NroVacunacion
        {
            get { return nroVacunacion; }
            set { nroVacunacion = value; }
        }

        private int nroGallina;
        public int NroGallina
        {
            get { return nroGallina; }
            set { nroGallina = value; }
        }

        private int nroVacuna;
        public int NroVacuna
        {
            get { return nroVacuna; }
            set { nroVacuna = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private DateTime proximaDosis;
        public DateTime ProximaDosis
        {
            get { return proximaDosis; }
            set { proximaDosis = value; }
        }

        private string observaciones;
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
    }
}
