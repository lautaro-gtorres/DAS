using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DetalleVenta
    {
        private int nroDetalleVenta;
        public int NroDetalleVenta
        {
            get { return nroDetalleVenta; }
            set { nroDetalleVenta = value; }
        }

        private int nroVenta;
        public int NroVenta
        {
            get { return nroVenta; }
            set { nroVenta = value; }
        }

        private string producto;
        public string Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        private decimal cantidad;
        public decimal Cantidad
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

        private decimal subtotal;
        public decimal Subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }
    }
}
