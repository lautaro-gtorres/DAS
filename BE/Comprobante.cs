using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Comprobante (no fiscal, tipo "X") de una venta. No es una tabla:
    /// se arma con los datos de la venta (SP ComprobanteVenta).
    /// </summary>
    public class Comprobante
    {
        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        // Punto de venta + número de venta (ej: 0001-00000042).
        private string nroComprobante;
        public string NroComprobante
        {
            get { return nroComprobante; }
            set { nroComprobante = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        // "Apellido, Nombre" o "Consumidor final".
        private string cliente;
        public string Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        // "Huevo" o "Gallina <tipo> <raza> (macho/hembra)".
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
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

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
