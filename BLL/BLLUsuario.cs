using BE;
using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Login y ABM de usuarios (baja lógica, contraseña en texto plano).
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLUsuario : IABMeable<BE.Usuario>
    {
        public const string RolAdministrador = "Administrador";

        MapperUsuario map = new MapperUsuario();

        // ---- Login ----

        // Si usuario y contraseña coinciden en la BD, inicia la sesión y devuelve true.
        public bool ValidarUsuario(string usuario, string password)
        {
            Usuario u = map.ValidarUsuario(usuario, password);
            if (u == null)
                return false;
            Sesion.Instancia.Iniciar(u);
            return true;
        }

        public void Logout()
        {
            Sesion.Instancia.Cerrar();
        }

        // ---- ABM ----

        public int Alta(Usuario usuario)
        {
            Validar(usuario, true);
            try
            {
                return map.Alta(usuario);
            }
            catch (SqlException ex)
            {
                TraducirError(ex, usuario);
                throw;
            }
        }

        // Baja lógica: el usuario queda inactivo y ya no puede iniciar sesión.
        public int Baja(int id)
        {
            if (Sesion.Instancia.HaySesion && Sesion.Instancia.UsuarioActual.IdUsuario == id)
                throw new ArgumentException("No puede darse de baja a sí mismo.");
            return map.Baja(id);
        }

        // Al modificar, si la contraseña viene vacía se conserva la que tenía
        // (los listados no la traen).
        public int Modificar(Usuario usuario)
        {
            Validar(usuario, false);
            try
            {
                return map.Modificar(usuario);
            }
            catch (SqlException ex)
            {
                TraducirError(ex, usuario);
                throw;
            }
        }

        public List<Usuario> Listar()
        {
            return map.Listar();
        }

        public List<Usuario> Buscar(Usuario filtro)
        {
            return map.Buscar(filtro);
        }

        public List<Rol> ListarRoles()
        {
            return map.ListarRoles();
        }

        // 2627 = nombre de usuario repetido (también entre los dados de baja);
        // 2601 = DNI repetido (índice único); 547 = el rol no existe (FK).
        void TraducirError(SqlException ex, Usuario usuario)
        {
            if (ex.Number == 2627)
                throw new ArgumentException("Ya existe el usuario " + usuario.NombreUsuario + " (puede estar dado de baja).");
            if (ex.Number == 2601)
                throw new ArgumentException("Ya hay un usuario con el DNI " + usuario.Dni + ".");
            if (ex.Number == 547)
                throw new ArgumentException("El rol seleccionado no existe.");
        }

        void Validar(Usuario usuario, bool esAlta)
        {
            if (usuario == null)
                throw new ArgumentException("No hay datos del usuario.");

            if (usuario.NombreUsuario == null)
                usuario.NombreUsuario = "";
            usuario.NombreUsuario = usuario.NombreUsuario.Trim();
            if (usuario.NombreUsuario == "")
                throw new ArgumentException("Ingrese el nombre de usuario.");
            if (usuario.NombreUsuario.Length > 50)
                throw new ArgumentException("El nombre de usuario no puede tener más de 50 caracteres.");
            if (string.IsNullOrEmpty(usuario.Contrasena))
            {
                if (esAlta)
                    throw new ArgumentException("Ingrese la contraseña.");
                usuario.Contrasena = null;   // al modificar: se conserva la anterior
            }
            if (usuario.Contrasena != null && usuario.Contrasena.Length > 256)
                throw new ArgumentException("La contraseña no puede tener más de 256 caracteres.");
            if (usuario.IdRol <= 0)
                throw new ArgumentException("Seleccione el rol.");

            // ---- Datos personales ----
            usuario.Nombre = Validaciones.SacarEspacios(usuario.Nombre);
            if (usuario.Nombre == null)
                throw new ArgumentException("Ingrese el nombre.");
            if (usuario.Nombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener más de 50 caracteres.");

            usuario.Apellido = Validaciones.SacarEspacios(usuario.Apellido);
            if (usuario.Apellido == null)
                throw new ArgumentException("Ingrese el apellido.");
            if (usuario.Apellido.Length > 50)
                throw new ArgumentException("El apellido no puede tener más de 50 caracteres.");

            // DNI, fecha de nacimiento, teléfono y e-mail son opcionales.
            usuario.Dni = Validaciones.SacarEspacios(usuario.Dni);
            if (usuario.Dni != null)
            {
                // Se acepta con puntos (12.345.678) y se guarda solo con los números.
                usuario.Dni = usuario.Dni.Replace(".", "");
                foreach (char c in usuario.Dni)
                {
                    if (!char.IsDigit(c))
                        throw new ArgumentException("El DNI solo puede tener números.");
                }
                if (usuario.Dni.Length < 7 || usuario.Dni.Length > 8)
                    throw new ArgumentException("El DNI tiene que tener 7 u 8 números.");
            }

            if (usuario.FechaNacimiento != null)
            {
                if (usuario.FechaNacimiento.Value.Date > DateTime.Today)
                    throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
                if (usuario.FechaNacimiento.Value.Year < 1900)
                    throw new ArgumentException("La fecha de nacimiento no es válida.");
            }

            usuario.Telefono = Validaciones.SacarEspacios(usuario.Telefono);
            if (usuario.Telefono != null && usuario.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");

            usuario.Email = Validaciones.SacarEspacios(usuario.Email);
            if (usuario.Email != null)
            {
                if (usuario.Email.Length > 100)
                    throw new ArgumentException("El e-mail no puede tener más de 100 caracteres.");
                if (!Validaciones.EsEmailValido(usuario.Email))
                    throw new ArgumentException("El e-mail no es válido.");
            }
        }
    }
}
