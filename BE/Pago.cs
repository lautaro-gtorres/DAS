using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Pago
    {
		private int nroPago;

		public int NroPago
		{
			get { return nroPago; }
			set { nroPago = value; }
		}
		private int nroLote;

		public int NroLote
		{
			get { return nroLote; }
			set { nroLote = value; }
		}
		private string ubicacion;

		public string Ubicacion
		{
			get { return ubicacion; }
			set { ubicacion = value; }
		}

		private int mt2Total;

		public int Mt2Total
		{
			get { return mt2Total; }
			set { mt2Total = value; }
		}

		private int precioMt2;

		public int PrecioMt2
		{
			get { return precioMt2; }
			set { precioMt2 = value; }
		}

		private int total;

		public int Total
		{
			get { return total; }
			set { total = value; }
		}

	}
}
