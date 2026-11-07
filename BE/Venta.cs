using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Venta: por ahora una venta es una sola línea (un tipo de producto).
    /// </summary>
    public class Venta
    {
        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private int idTipoProducto;
        public int IdTipoProducto
        {
            get { return idTipoProducto; }
            set { idTipoProducto = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        // Huevo o Pollo.
        private string tipoProducto;
        public string TipoProducto
        {
            get { return tipoProducto; }
            set { tipoProducto = value; }
        }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private decimal precioUnitario;
        public decimal PrecioUnitario
        {
            get { return precioUnitario; }
            set { precioUnitario = value; }
        }

        // Columna calculada en la base (Cantidad * PrecioUnitario): no se envía al guardar.
        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        // Nombre del cliente como texto (todavía no hay tabla Cliente).
        private string cliente;
        public string Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }
    }
}
