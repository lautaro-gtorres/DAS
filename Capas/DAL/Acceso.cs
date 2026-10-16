using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Acceso
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd;

        public void Conectar()
        {
            cn.ConnectionString = @"Data Source=rochi\SQLEXPRESS;Initial Catalog=Facultad2;Integrated Security=True;";
            cn.Open();
        }

        public void Desconectar()
        {
            cn.Close();
            cn.Dispose();
        }

        public DataTable Leer(string query, SqlParameter[] parametros)
        {
            Conectar();

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = query;

            if (parametros != null)
            {
                cmd.Parameters.AddRange(parametros);
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable tabla = new DataTable();
            adapter.Fill(tabla);

            Desconectar();

            return tabla;
        }

        public int Escribir(string query, SqlParameter[] parametros)
        {
            int fa = 0;

            Conectar();

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = query;
            cmd.Parameters.AddRange(parametros);

            fa = cmd.ExecuteNonQuery();

            Desconectar();

            return fa;
        }
    }
}
