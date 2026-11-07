using System;

namespace BLL
{
    /// <summary>
    /// Usuario logueado en la aplicación (patrón Singleton).
    /// Cualquier formulario lo consulta con Sesion.Instancia.UsuarioActual.
    /// </summary>
    public sealed class Sesion
    {
        private static readonly Lazy<Sesion> instancia = new Lazy<Sesion>(() => new Sesion());

        public static Sesion Instancia
        {
            get { return instancia.Value; }
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

        // Solo BLLUsuario.Login puede iniciar la sesión.
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
