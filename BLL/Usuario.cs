using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Usuario
    {
        public const string TipoAdministrador = "Administrador";
        public const string TipoCliente = "Cliente";

        Mapper.MapperUsuario mapper = new Mapper.MapperUsuario();

        public BE.Usuario Login(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Ingresá usuario y contraseña.");

            BE.Usuario usuario = mapper.Login(nombreUsuario.Trim(), password);
            if (usuario == null)
                throw new ArgumentException("Usuario o contraseña incorrectos.");
            if (!usuario.Estado)
                throw new ArgumentException("El usuario está inactivo. Contactá al administrador.");

            return usuario;
        }

        // Alta desde la pantalla de registro: siempre Cliente, activo y con saldo 0.
        public int RegistrarCliente(BE.Usuario usuario, string confirmacionPassword)
        {
            usuario.Tipo = TipoCliente;
            usuario.Estado = true;
            usuario.Saldo = 0;

            Validar(usuario, true);
            if (usuario.Password != confirmacionPassword)
                throw new ArgumentException("Las contraseñas no coinciden.");
            ValidarUsuarioDisponible(usuario.NombreUsuario);

            return mapper.Alta(usuario);
        }

        public BE.Usuario ObtenerPorId(int idUsuario)
        {
            return mapper.ObtenerPorId(idUsuario);
        }

        public BE.Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            return mapper.ObtenerPorNombreUsuario(nombreUsuario);
        }

        // ---- ABM (administrador) ----

        public int Alta(BE.Usuario usuario)
        {
            Validar(usuario, true);
            if (usuario.Saldo < 0)
                throw new ArgumentException("El saldo inicial no puede ser negativo.");
            ValidarUsuarioDisponible(usuario.NombreUsuario);
            return mapper.Alta(usuario);
        }

        public int Modificar(BE.Usuario usuario)
        {
            if (usuario.IdUsuario <= 0)
                throw new ArgumentException("Seleccioná un usuario de la lista.");
            Validar(usuario, false);
            return mapper.Modificar(usuario);
        }

        public int Baja(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Seleccioná un usuario de la lista.");
            return mapper.Baja(idUsuario);
        }

        public List<BE.Usuario> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Usuario> Buscar(BE.Usuario filtro)
        {
            return mapper.Buscar(filtro);
        }

        // ---- Validaciones ----

        void Validar(BE.Usuario usuario, bool exigirPassword)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Apellido))
                throw new ArgumentException("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Dni) || !usuario.Dni.All(char.IsDigit)
                || usuario.Dni.Length < 7 || usuario.Dni.Length > 8)
                throw new ArgumentException("El DNI debe tener 7 u 8 números, sin puntos.");
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || usuario.NombreUsuario.Contains(" "))
                throw new ArgumentException("El nombre de usuario es obligatorio y no puede tener espacios.");
            if (exigirPassword || !string.IsNullOrEmpty(usuario.Password))
            {
                if (string.IsNullOrEmpty(usuario.Password) || usuario.Password.Length < 4)
                    throw new ArgumentException("La contraseña debe tener al menos 4 caracteres.");
            }
            if (usuario.Tipo != TipoAdministrador && usuario.Tipo != TipoCliente)
                throw new ArgumentException("Seleccioná el tipo de usuario.");
        }

        void ValidarUsuarioDisponible(string nombreUsuario)
        {
            if (mapper.ObtenerPorNombreUsuario(nombreUsuario) != null)
                throw new ArgumentException("Ese nombre de usuario ya está en uso.");
        }
    }
}
