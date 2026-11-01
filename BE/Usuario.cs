using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        private int nroUsuario;
        public int NroUsuario
        {
            get { return nroUsuario; }
            set { nroUsuario = value; }
        }


        private string nombreUsuario;
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        private string contrasenia;

        public string Contrasenia
        {
            get { return contrasenia; }
            set { contrasenia = value; }
        }

        private string rol;
        public string Rol
        {
            get { return rol; }
            set { rol = value; }
        }

        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

    }
}
