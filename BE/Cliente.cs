using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Cliente. Hereda de Persona nombre, apellido, teléfono y e-mail.
    /// </summary>
    public class Cliente : Persona
    {
        public Cliente()
        {
        }

        public Cliente(string nombre, string apellido) : base(nombre, apellido)
        {
        }

        private int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }

        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public override string TipoPersona()
        {
            return "Cliente";
        }
    }
}
