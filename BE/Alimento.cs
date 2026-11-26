using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Alimento.
    /// </summary>
    public class Alimento
    {
        private int idAlimento;
        public int IdAlimento
        {
            get { return idAlimento; }
            set { idAlimento = value; }
        }

        private string nombreAlimento;
        public string NombreAlimento
        {
            get { return nombreAlimento; }
            set { nombreAlimento = value; }
        }

        // Stock disponible (no puede ser negativo).
        private decimal cantidadDisponible;
        public decimal CantidadDisponible
        {
            get { return cantidadDisponible; }
            set { cantidadDisponible = value; }
        }

        private int idProveedor;
        public int IdProveedor
        {
            get { return idProveedor; }
            set { idProveedor = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        // Nombre del proveedor.
        private string proveedor;
        public string Proveedor
        {
            get { return proveedor; }
            set { proveedor = value; }
        }
    }
}
