using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class MapperUsuario : IMapper<BE.Usuario>
    {
        DAL.Acceso acc = new DAL.Acceso();

        public BE.Usuario Login(string nombreUsuario, string password)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@usuario", nombreUsuario);
            parametros[1] = new SqlParameter("@password", password);
            DataTable dt = acc.Leer("LoginUsuario", parametros);
            if (dt.Rows.Count == 0)
                return null;
            return Mapear(dt.Rows[0]);
        }

        public BE.Usuario ObtenerPorId(int idUsuario)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idUsuario", idUsuario);
            DataTable dt = acc.Leer("ObtenerUsuarioPorId", parametros);
            if (dt.Rows.Count == 0)
                return null;
            return Mapear(dt.Rows[0]);
        }

        public BE.Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@usuario", nombreUsuario);
            DataTable dt = acc.Leer("ObtenerUsuarioPorNombreUsuario", parametros);
            if (dt.Rows.Count == 0)
                return null;
            return Mapear(dt.Rows[0]);
        }

        public int Alta(BE.Usuario entidad)
        {
            int fa = 0;
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("@nombre", entidad.Nombre);
            parametros[1] = new SqlParameter("@apellido", entidad.Apellido);
            parametros[2] = new SqlParameter("@dni", entidad.Dni);
            parametros[3] = new SqlParameter("@usuario", entidad.NombreUsuario);
            parametros[4] = new SqlParameter("@password", entidad.Password);
            parametros[5] = new SqlParameter("@tipo", entidad.Tipo);
            parametros[6] = new SqlParameter("@estado", entidad.Estado);
            parametros[7] = new SqlParameter("@saldo", entidad.Saldo);
            fa = acc.Escribir("AltaUsuario", parametros);
            return fa;
        }

        public int Baja(int id)
        {
            int fa = 0;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idUsuario", id);
            fa = acc.Escribir("BajaUsuario", parametros);
            return fa;
        }

        public int Modificar(BE.Usuario entidad)
        {
            int fa = 0;
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("@idUsuario", entidad.IdUsuario);
            parametros[1] = new SqlParameter("@nombre", entidad.Nombre);
            parametros[2] = new SqlParameter("@apellido", entidad.Apellido);
            parametros[3] = new SqlParameter("@dni", entidad.Dni);
            parametros[4] = new SqlParameter("@usuario", entidad.NombreUsuario);
            // Contraseña vacía = el SP mantiene la actual
            parametros[5] = new SqlParameter("@password", (object)entidad.Password ?? DBNull.Value);
            parametros[6] = new SqlParameter("@tipo", entidad.Tipo);
            parametros[7] = new SqlParameter("@estado", entidad.Estado);
            fa = acc.Escribir("ModificarUsuario", parametros);
            return fa;
        }

        public List<BE.Usuario> Listar()
        {
            DataTable dt = acc.Leer("ListarUsuarios");
            return MapearLista(dt);
        }

        public List<BE.Usuario> Buscar(BE.Usuario filtro)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(filtro.NombreUsuario))
                parametros.Add(new SqlParameter("@usuario", filtro.NombreUsuario));
            if (!string.IsNullOrWhiteSpace(filtro.Dni))
                parametros.Add(new SqlParameter("@dni", filtro.Dni));
            if (!string.IsNullOrWhiteSpace(filtro.Apellido))
                parametros.Add(new SqlParameter("@apellido", filtro.Apellido));

            DataTable dt = acc.Leer("BuscarUsuario", parametros.ToArray());
            return MapearLista(dt);
        }

        // ---- Fila de la base -> entidad BE ----

        BE.Usuario Mapear(DataRow row)
        {
            BE.Usuario usuario = new BE.Usuario();
            usuario.IdUsuario = Convert.ToInt32(row["idUsuario"]);
            usuario.Nombre = row["nombre"].ToString();
            usuario.Apellido = row["apellido"].ToString();
            usuario.Dni = row["dni"].ToString();
            usuario.NombreUsuario = row["usuario"].ToString();
            usuario.Saldo = Convert.ToDouble(row["saldo"]);
            usuario.Estado = Convert.ToBoolean(row["estado"]);
            usuario.Tipo = row["tipo"].ToString();
            // Los SP de consulta no devuelven la contraseña.
            if (row.Table.Columns.Contains("password"))
                usuario.Password = row["password"].ToString();
            return usuario;
        }

        List<BE.Usuario> MapearLista(DataTable tabla)
        {
            List<BE.Usuario> usuarios = new List<BE.Usuario>();
            foreach (DataRow row in tabla.Rows)
            {
                usuarios.Add(Mapear(row));
            }
            return usuarios;
        }
    }
}
