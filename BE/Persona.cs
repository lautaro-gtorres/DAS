using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Datos comunes de las personas del sistema. Clase base de Cliente y Usuario.
    /// </summary>
    public abstract class Persona
    {
        // Solo las clases derivadas pueden crear una Persona.
        protected Persona()
        {
        }

        protected Persona(string nombre, string apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
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

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // "Cliente" o "Usuario": cada clase derivada dice qué es.
        public abstract string TipoPersona();

        // Cómo se muestra la persona; las clases derivadas lo pueden redefinir.
        public virtual string NombreCompleto()
        {
            return Apellido + ", " + Nombre;
        }

        // Lo que se muestra en un ComboBox o ListBox.
        public override string ToString()
        {
            return NombreCompleto();
        }
    }
}
