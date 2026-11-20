using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Viaje
    {
        private int idViaje;
        public int IdViaje
        {
            get { return idViaje; }
            set { idViaje = value; }
        }

        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { idVehiculo = value; }
        }

        private int idPasajero;
        public int IdPasajero
        {
            get { return idPasajero; }
            set { idPasajero = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private string destino;
        public string Destino
        {
            get { return destino; }
            set { destino = value; }
        }

        private decimal kilometro;
        public decimal Kilometro
        {
            get { return kilometro; }
            set { kilometro = value; }
        }

        private decimal importe;
        public decimal Importe
        {
            get { return importe; }
            set { importe = value; }
        }
    }
}
