using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DetallePedido
    {
        private int nroDetallePedido;
        public int NroDetallePedido
        {
            get { return nroDetallePedido; }
            set { nroDetallePedido = value; }
        }

        private int nroPedido;
        public int NroPedido
        {
            get { return nroPedido; }
            set { nroPedido = value; }
        }

        private int nroAlimento;
        public int NroAlimento
        {
            get { return nroAlimento; }
            set { nroAlimento = value; }
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
