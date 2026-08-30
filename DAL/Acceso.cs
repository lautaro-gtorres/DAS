using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd;
        
        public void Conectar()
        {
                cn.ConnectionString = "Data Source=LAUTARO-THERA\\MSSQLSERVER01;Initial Catalog=Prueba;Integrated Security=True"; // Implement connection logic here
                cn.Open();
        }
        public void Desconectar()
        {
            cn.Close();
            cn.Dispose();
        }
        public DataTable Leer(string storedProcedure)
        {
            DataTable dt = new DataTable();
            Conectar();
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            Desconectar();
            return dt;
        }
        public int Escribir(string query, SqlParameter[] parameters)
        {
            int fa = 0;
            Conectar();
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = query;
            cmd.Parameters.AddRange(parameters);
            fa = cmd.ExecuteNonQuery();
            Desconectar();
            return fa;
        }
    }
}
