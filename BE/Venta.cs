using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Venta
    {
        private int nroVenta;
        public int NroVenta
        {
            get { return nroVenta; }
            set { nroVenta = value; }
        }

        private int nroCliente;
        public int NroCliente
        {
            get { return nroCliente; }
            set { nroCliente = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        private string medioPago;
        public string MedioPago
        {
            get { return medioPago; }
            set { medioPago = value; }
        }
    }
}
