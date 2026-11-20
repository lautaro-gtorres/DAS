using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd;
        SqlTransaction tr;

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
            Conectar();
            try
            {
                tr = cn.BeginTransaction();
                cmd = cn.CreateCommand();
                cmd.Connection = cn;
                cmd.Transaction = tr;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = query;
                cmd.Parameters.AddRange(parameters);
                fa = cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception)
            {
                tr.Rollback();
                throw;   // se relanza para que el formulario muestre el error
            }
            finally
            {
                Desconectar();
            }
            return fa;
        }

        // ---- XML ----

        // Llena un DataSet con el SP y lo guarda en el archivo XML
        public void EscribirXml(string storedProcedure, string tabla, string ruta)
        {
            DataSet DS = new DataSet();
            try
            {
                Conectar();
                SqlDataAdapter DA = new SqlDataAdapter(storedProcedure, cn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.Fill(DS, tabla);
            }
            finally
            {
                Desconectar();
            }
            DS.WriteXml(ruta);
        }

        // Lee el archivo XML y lo devuelve en un DataSet
        public DataSet LeerXml(string ruta)
        {
            DataSet DS = new DataSet();
            DS.ReadXml(ruta);
            return DS;
        }
    }
}
