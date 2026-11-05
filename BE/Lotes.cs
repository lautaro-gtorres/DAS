using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Lotes
	{
		private int nroLote;

		public int NroLote
		{
			get { return nroLote; }
			set { nroLote = value; }
		}


		private string nombrePropietario;

		public string NombrePropietario
		{
			get { return nombrePropietario; }
			set { nombrePropietario = value; }
		}
		private int medidaFrente;

		public int MedidaFrente
		{
			get { return medidaFrente; }
			set { medidaFrente = value; }
		}
		private int medidaFondo;

		public int MedidaFondo
		{
			get { return medidaFondo; }
			set { medidaFondo = value; }
		}
		private int totalMts2;

        public Lotes(int nroLote, string nombrePropietario, int medidaFrente, int medidaFondo, int totalMts2)
        {
            this.nroLote = nroLote;
            this.nombrePropietario = nombrePropietario;
            this.medidaFrente = medidaFrente;
            this.medidaFondo = medidaFondo;
            this.totalMts2 = totalMts2;
        }

        public int TotalMts2
		{
			get { return totalMts2; }
			set { totalMts2 = value; }
		}

		public Lotes()
        {
        }	


    }
}
