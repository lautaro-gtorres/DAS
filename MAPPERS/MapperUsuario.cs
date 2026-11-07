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
        Acceso acc = new Acceso();

        public bool ValidarUsuario(string usuario, string password)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@Usuario", usuario);
            parametros[1] = new SqlParameter("@Password", password);
            DataTable dt = acc.Leer("SP_ValidarUsuario", parametros);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
