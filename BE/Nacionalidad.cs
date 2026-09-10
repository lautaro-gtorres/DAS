using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Nacionalidad
    {
		private int idNacionalidad;

		public int IdNacionalidad
		{
			get { return idNacionalidad; }
			set { idNacionalidad = value; }
		}

		private string descNacionalidad;

		public string DescNacionalidad	
        {
			get { return descNacionalidad; }
			set { descNacionalidad = value; }
		}

	}
}
