using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Vacunacion: se vacuna por corral.
    /// </summary>
    public class Vacunacion
    {
        private int idVacunacion;
        public int IdVacunacion
        {
            get { return idVacunacion; }
            set { idVacunacion = value; }
        }

        // Nombre de la vacuna.
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private int idCorral;
        public int IdCorral
        {
            get { return idCorral; }
            set { idCorral = value; }
        }

        // Número del corral (lo completa el mapper con un JOIN, para mostrar en listados).
        private string nroCorral;
        public string NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }

        private DateTime fechaAplicacion;
        public DateTime FechaAplicacion
        {
            get { return fechaAplicacion; }
            set { fechaAplicacion = value; }
        }

        // Opcional en la base.
        private DateTime? proximaAplicacion;
        public DateTime? ProximaAplicacion
        {
            get { return proximaAplicacion; }
            set { proximaAplicacion = value; }
        }
    }
}
