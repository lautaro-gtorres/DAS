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
        }
    }
}
