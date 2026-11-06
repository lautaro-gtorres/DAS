using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Pedido
    {
        private int nroPedido;
        public int NroPedido
        {
            get { return nroPedido; }
            set { nroPedido = value; }
        }

        private int nroProveedor;
        public int NroProveedor
        {
            get { return nroProveedor; }
            set { nroProveedor = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
