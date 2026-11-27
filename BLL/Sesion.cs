using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Usuario logueado en la aplicación (patrón Singleton).
    // Cualquier formulario lo consulta con Sesion.Instancia.UsuarioActual.
    public class Sesion
    {
        static Sesion instancia;

        public static Sesion Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new Sesion();
                return instancia;
            }
        }

        private Sesion()
        {
        }

        private BE.Usuario usuarioActual;
        public BE.Usuario UsuarioActual
        {
            get { return usuarioActual; }
        }

        public bool HaySesion
        {
            get { return usuarioActual != null; }
        }

        public bool EsAdministrador
        {
            get { return HaySesion && usuarioActual.Rol == BLLUsuario.RolAdministrador; }
        }

        // Solo BLLUsuario.ValidarUsuario inicia la sesión.
        internal void Iniciar(BE.Usuario usuario)
        {
            usuarioActual = usuario;
        }

        public void Cerrar()
        {
            usuarioActual = null;
        }
    }
}
