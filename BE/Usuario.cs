using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
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

        // ---- Datos personales (formulario UsuarioNuevo) ----

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
    }
}
