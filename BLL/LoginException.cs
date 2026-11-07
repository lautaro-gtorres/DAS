using System;

namespace BLL
{
    /// <summary>
    /// Error esperado del login (datos vacíos, credenciales incorrectas, usuario inactivo).
    /// El mensaje se puede mostrar tal cual al usuario.
    /// </summary>
    public class LoginException : Exception
    {
        public LoginException(string mensaje) : base(mensaje)
        {
        }
    }
}
