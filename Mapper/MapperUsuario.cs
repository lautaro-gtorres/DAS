using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    // Convierte las filas que devuelve la base en entidades BE.Usuario.
    public class MapperUsuario
    {
        public BE.Usuario Mapear(DataRow row)
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

        public List<BE.Usuario> MapearLista(DataTable tabla)
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
