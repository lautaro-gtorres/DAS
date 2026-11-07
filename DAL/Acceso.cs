using BE;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var cs = ConfigurationManager.ConnectionStrings["DBPollos"];
            if (cs == null)
                throw new InvalidOperationException("Falta la cadena de conexión 'DBPollos'. Copiá ConnectionStrings.example.config como ConnectionStrings.config y completalo.");
            cn.ConnectionString = cs.ConnectionString;
            cn.Open();
        }
        public void Desconectar()
        {
            cn.Close();
            cn.Dispose();
        }
        public DataTable Leer(string storedProcedure, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            Conectar();
            cmd = cn.CreateCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedure;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
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
            cmd = cn.CreateCommand();
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
