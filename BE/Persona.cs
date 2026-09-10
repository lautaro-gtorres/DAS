using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class Persona
	{
		private int idPersona;

		public int IdPersona
        {
			get { return idPersona; }
			set { idPersona = value; }
		}


		private string nombre;

		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}
		private string apellido;

		public string Apellido
		{
			get { return apellido; }
			set { apellido = value; }
		}
		private int edad;

		public int Edad
		{
			get { return edad; }
			set { edad = value; }
		}
		private string sexo;

		public string Sexo
		{
			get { return sexo; }
			set { sexo = value; }
		}
		private int idNacionalidad;

		public int Idnacionalidad
		{
			get { return idNacionalidad; }
			set { idNacionalidad = value; }
		}
		private int idProfesion;

		public int IdProfesion
		{
			get { return idProfesion; }
			set { idProfesion = value; }
		}






	}
}
