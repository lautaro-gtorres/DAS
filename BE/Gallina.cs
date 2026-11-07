using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Gallina.
    /// </summary>
    public class Gallina
    {
        public const char Macho = 'M';
        public const char Hembra = 'H';

        private int idGallina;
        public int IdGallina
        {
            get { return idGallina; }
            set { idGallina = value; }
        }

        // Código visible de la gallina. Es único.
        private string codGallina;
        public string CodGallina
        {
            get { return codGallina; }
            set { codGallina = value; }
        }

        private int idRaza;
        public int IdRaza
        {
            get { return idRaza; }
            set { idRaza = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        private string raza;
        public string Raza
        {
            get { return raza; }
            set { raza = value; }
        }

        private int idTipoGallina;
        public int IdTipoGallina
        {
            get { return idTipoGallina; }
            set { idTipoGallina = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        // Ponedora, Engorde o Gallo.
        private string tipoGallina;
        public string TipoGallina
        {
            get { return tipoGallina; }
            set { tipoGallina = value; }
        }

        // 'M' (macho) o 'H' (hembra). Ver constantes Macho y Hembra.
        private char sexo;
        public char Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }

        // Opcional en la base.
        private DateTime? fechaNacimiento;
        public DateTime? FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        private int idCorral;
        public int IdCorral
        {
            get { return idCorral; }
            set { idCorral = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        private string nroCorral;
        public string NroCorral
        {
            get { return nroCorral; }
            set { nroCorral = value; }
        }

        // Observaciones (hasta 500 caracteres).
        private string detalle;
        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }
    }
}
