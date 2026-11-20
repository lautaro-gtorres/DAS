using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Vehiculo
    {
        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { idVehiculo = value; }
        }

        private string marca;
        public string Marca
        {
            get { return marca; }
            set { marca = value; }
        }

        private string modelo;
        public string Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        private string patente;
        public string Patente
        {
            get { return patente; }
            set { patente = value; }
        }

        private bool disponible;
        public bool Disponible
        {
            get { return disponible; }
            set { disponible = value; }
        }

        // Lo que muestra el ComboBox
        public override string ToString()
        {
            return patente + " - " + marca + " " + modelo;
        }
    }
}
