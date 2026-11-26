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
    public class MapperUsuario
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

        BE.Usuario Mapear(DataRow row)
        {
            BE.Usuario usuario = new BE.Usuario();
            usuario.IdUsuario = Convert.ToInt32(row["IdUsuario"]);
            usuario.NombreUsuario = row["NombreUsuario"].ToString();
            usuario.IdRol = Convert.ToInt32(row["IdRol"]);
            // La descripción del rol solo viene si el SP hace JOIN con Rol (Database/01_Login.sql).
            if (row.Table.Columns.Contains("Rol"))
                usuario.Rol = row["Rol"].ToString();
            usuario.Activo = Convert.ToBoolean(row["Activo"]);
            return usuario;
        }
    }
}
