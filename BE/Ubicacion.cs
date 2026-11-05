using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Ubicacion
    {
		private string idUbicacion;

		public string IdUbicacion
		{
			get { return idUbicacion; }
			set { idUbicacion = value; }
		}

		private string descripcion;

		public string Descripcion
		{
			get { return descripcion; }
			set { descripcion = value; }
		}

		private int valorMt2;

        public Ubicacion(string idUbicacion, string descripcion, int valorMt2)
        {
            this.idUbicacion = idUbicacion;
            this.descripcion = descripcion;
            this.valorMt2 = valorMt2;
        }

        public int ValorMt2
		{
			get { return valorMt2; }
			set { valorMt2 = value; }
		}

		public Ubicacion()
        {
        }	

    }
}
