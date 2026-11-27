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
            Validar(usuario);
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

        // La contraseña es obligatoria también al modificar (los listados no la traen).
        public int Modificar(Usuario usuario)
        {
            Validar(usuario);
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

        // 2627 = nombre de usuario repetido (también entre los dados de baja); 547 = el rol no existe (FK).
        void TraducirError(SqlException ex, Usuario usuario)
        {
            if (ex.Number == 2627)
                throw new ArgumentException("Ya existe el usuario " + usuario.NombreUsuario + " (puede estar dado de baja).");
            if (ex.Number == 547)
                throw new ArgumentException("El rol seleccionado no existe.");
        }

        void Validar(Usuario usuario)
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
                throw new ArgumentException("Ingrese la contraseña.");
            if (usuario.Contrasena.Length > 256)
                throw new ArgumentException("La contraseña no puede tener más de 256 caracteres.");
            if (usuario.IdRol <= 0)
                throw new ArgumentException("Seleccione el rol.");

            // ---- Datos personales ----
            usuario.Nombre = SacarEspacios(usuario.Nombre);
            if (usuario.Nombre == null)
                throw new ArgumentException("Ingrese el nombre.");
            if (usuario.Nombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener más de 50 caracteres.");

            usuario.Apellido = SacarEspacios(usuario.Apellido);
            if (usuario.Apellido == null)
                throw new ArgumentException("Ingrese el apellido.");
            if (usuario.Apellido.Length > 50)
                throw new ArgumentException("El apellido no puede tener más de 50 caracteres.");

            // DNI, fecha de nacimiento, teléfono y e-mail son opcionales.
            usuario.Dni = SacarEspacios(usuario.Dni);
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

            usuario.Telefono = SacarEspacios(usuario.Telefono);
            if (usuario.Telefono != null && usuario.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");

            usuario.Email = SacarEspacios(usuario.Email);
            if (usuario.Email != null)
            {
                if (usuario.Email.Length > 100)
                    throw new ArgumentException("El e-mail no puede tener más de 100 caracteres.");
                if (!EsEmailValido(usuario.Email))
                    throw new ArgumentException("El e-mail no es válido.");
            }
        }

        // Control simple: algo antes de la @ y un punto después.
        bool EsEmailValido(string email)
        {
            int arroba = email.IndexOf('@');
            if (arroba < 1)
                return false;
            if (email.LastIndexOf('.') < arroba + 2)
                return false;
            if (email.EndsWith("."))
                return false;
            return true;
        }

        // Devuelve el texto sin espacios de los costados, o null si está vacío.
        string SacarEspacios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;
            return texto.Trim();
        }
    }
}
