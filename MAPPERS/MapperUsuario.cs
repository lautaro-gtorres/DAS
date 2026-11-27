using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    public class MapperUsuario : IMapper<BE.Usuario>
    {
        // Devuelve el usuario si el nombre y la contraseña coinciden, o null si no.
        public BE.Usuario ValidarUsuario(string usuario, string password)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@Usuario", usuario);
            parametros[1] = new SqlParameter("@Password", password);
            DataTable dt = Acceso.Instancia.Leer("SP_ValidarUsuario", parametros);
            if (dt.Rows.Count == 0)
                return null;
            return Mapear(dt.Rows[0]);
        }

        public int Alta(BE.Usuario usuario)
        {
            SqlParameter[] parametros = new SqlParameter[9];
            parametros[0] = new SqlParameter("@nombreUsuario", usuario.NombreUsuario);
            parametros[1] = new SqlParameter("@contrasena", usuario.Contrasena);
            parametros[2] = new SqlParameter("@idRol", usuario.IdRol);
            parametros[3] = new SqlParameter("@nombre", Conversion.TextoONulo(usuario.Nombre));
            parametros[4] = new SqlParameter("@apellido", Conversion.TextoONulo(usuario.Apellido));
            parametros[5] = new SqlParameter("@dni", Conversion.TextoONulo(usuario.Dni));
            parametros[6] = ParametroFechaNacimiento(usuario);
            parametros[7] = new SqlParameter("@telefono", Conversion.TextoONulo(usuario.Telefono));
            parametros[8] = new SqlParameter("@email", Conversion.TextoONulo(usuario.Email));
            return Acceso.Instancia.Escribir("AltaUsuario", parametros);
        }

        // Baja lógica: el usuario queda con Activo = 0 y ya no puede entrar.
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idUsuario", id);
            return Acceso.Instancia.Escribir("BajaUsuario", parametros);
        }

        public int Modificar(BE.Usuario usuario)
        {
            SqlParameter[] parametros = new SqlParameter[10];
            parametros[0] = new SqlParameter("@idUsuario", usuario.IdUsuario);
            parametros[1] = new SqlParameter("@nombreUsuario", usuario.NombreUsuario);
            // NULL = se conserva la contraseña anterior.
            parametros[2] = new SqlParameter("@contrasena", Conversion.TextoONulo(usuario.Contrasena));
            parametros[3] = new SqlParameter("@idRol", usuario.IdRol);
            parametros[4] = new SqlParameter("@nombre", Conversion.TextoONulo(usuario.Nombre));
            parametros[5] = new SqlParameter("@apellido", Conversion.TextoONulo(usuario.Apellido));
            parametros[6] = new SqlParameter("@dni", Conversion.TextoONulo(usuario.Dni));
            parametros[7] = ParametroFechaNacimiento(usuario);
            parametros[8] = new SqlParameter("@telefono", Conversion.TextoONulo(usuario.Telefono));
            parametros[9] = new SqlParameter("@email", Conversion.TextoONulo(usuario.Email));
            return Acceso.Instancia.Escribir("ModificarUsuario", parametros);
        }

        // Solo usuarios activos, sin la contraseña.
        public List<BE.Usuario> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarUsuarios"));
        }

        // Busca por nombre de usuario (contiene el texto de filtro.NombreUsuario).
        public List<BE.Usuario> Buscar(BE.Usuario filtro)
        {
            string nombre = filtro.NombreUsuario;
            if (nombre == null)
                nombre = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombreUsuario", nombre);
            return Mapear(Acceso.Instancia.Leer("BuscarUsuario", parametros));
        }

        // Catálogo para el ComboBox de rol.
        public List<BE.Rol> ListarRoles()
        {
            List<BE.Rol> lista = new List<BE.Rol>();
            foreach (DataRow row in Acceso.Instancia.Leer("ListarRoles").Rows)
            {
                BE.Rol rol = new BE.Rol();
                rol.IdRol = Convert.ToInt32(row["IdRol"]);
                rol.Descripcion = row["Descripcion"].ToString();
                lista.Add(rol);
            }
            return lista;
        }

        SqlParameter ParametroFechaNacimiento(BE.Usuario usuario)
        {
            if (usuario.FechaNacimiento == null)
                return new SqlParameter("@fechaNacimiento", DBNull.Value);
            return new SqlParameter("@fechaNacimiento", usuario.FechaNacimiento.Value.Date);
        }

        // ---- Filas de la base -> entidades BE ----

        List<BE.Usuario> Mapear(DataTable dt)
        {
            List<BE.Usuario> lista = new List<BE.Usuario>();
            foreach (DataRow row in dt.Rows)
                lista.Add(Mapear(row));
            return lista;
        }

        BE.Usuario Mapear(DataRow row)
        {
            BE.Usuario usuario = new BE.Usuario();
            usuario.IdUsuario = Convert.ToInt32(row["IdUsuario"]);
            usuario.NombreUsuario = row["NombreUsuario"].ToString();
            usuario.IdRol = Convert.ToInt32(row["IdRol"]);
            usuario.Activo = Convert.ToBoolean(row["Activo"]);
            // La descripción del rol solo viene si el SP hace JOIN con Rol.
            if (row.Table.Columns.Contains("Rol"))
                usuario.Rol = row["Rol"].ToString();

            // Los datos personales vienen en ListarUsuarios y BuscarUsuario (el login no los trae).
            if (row.Table.Columns.Contains("Nombre"))
            {
                usuario.Nombre = Conversion.TextoDeFila(row, "Nombre");
                usuario.Apellido = Conversion.TextoDeFila(row, "Apellido");
                usuario.Dni = Conversion.TextoDeFila(row, "DNI");
                if (row["FechaNacimiento"] == DBNull.Value)
                    usuario.FechaNacimiento = null;
                else
                    usuario.FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]);
                usuario.Telefono = Conversion.TextoDeFila(row, "Telefono");
                usuario.Email = Conversion.TextoDeFila(row, "Email");
            }
            return usuario;
        }
    }
}
