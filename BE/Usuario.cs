using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Usuario. Hereda de Persona nombre, apellido, teléfono y e-mail.
    /// </summary>
    public class Usuario : Persona
    {
        public Usuario()
        {
        }

        public Usuario(string nombreUsuario, string contrasena)
        {
            this.nombreUsuario = nombreUsuario;
            this.contrasena = contrasena;
        }

        private int idUsuario;
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        private string nombreUsuario;
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        // Contraseña en texto plano (los listados no la traen).
        private string contrasena;
        public string Contrasena
        {
            get { return contrasena; }
            set { contrasena = value; }
        }

        private int idRol;
        public int IdRol
        {
            get { return idRol; }
            set { idRol = value; }
        }

        // Descripción del rol (tabla Rol): "Administrador" u "Operario".
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

        private string dni;
        public string Dni
        {
            get { return dni; }
            set { dni = value; }
        }

        // Opcional en la base.
        private DateTime? fechaNacimiento;
        public DateTime? FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        public override string TipoPersona()
        {
            return "Usuario";
        }

        // Un usuario puede no tener nombre y apellido cargados: entonces se muestra su nombre de usuario.
        public override string NombreCompleto()
        {
            if (string.IsNullOrEmpty(Apellido))
                return NombreUsuario;
            return base.NombreCompleto() + " (" + NombreUsuario + ")";
        }
    }
}
