using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class Acceso
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd;

        public void Conectar()
        {
            cn.ConnectionString = "Data Source=LAUTARO-THERA\\MSSQLSERVER01;Initial Catalog=Prueba;Integrated Security=True";
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
            try
            {
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
            }
            finally
            {
                Desconectar();   // se cierra aunque falle el SP
            }
            return dt;
        }
        public int Escribir(string query, SqlParameter[] parameters)
        {
            int fa = 0;
            try
            {
                Conectar();
                cmd = cn.CreateCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = query;
                cmd.Parameters.AddRange(parameters);
                fa = cmd.ExecuteNonQuery();
            }
            finally
            {
                Desconectar();
            }
            return fa;
        }

        // ---- XML ----

        public void EscribirXml<T>(List<T> lista, string ruta)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            using (StreamWriter sw = new StreamWriter(ruta))
            {
                xs.Serialize(sw, lista);
            }
        }

        public List<T> LeerXml<T>(string ruta)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            using (StreamReader sr = new StreamReader(ruta))
            {
                return (List<T>)xs.Deserialize(sr);
            }
        }
    }
}
