using BE;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;

            // La base de Azure es "serverless": si no se usa por un rato se pausa sola, y la
            // primera conexión falla con el error 40613 ("is not currently available")
            // mientras vuelve a arrancar. Se reintenta cada 10 segundos, hasta 1 minuto.
            int intentos = 0;
            while (true)
            {
                try
                {
                    cn.Open();
                    return;
                }
                catch (SqlException ex) when (ex.Number == 40613 && intentos < 6)
                {
                    intentos++;
                    System.Threading.Thread.Sleep(10000);
                }
            }
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
                // Si el SP falla, la conexión se cierra igual y la próxima llamada no se rompe.
                Desconectar();
            }
            return dt;
        }

        // Una sola escritura dentro de su propia transacción (como en el TP1).
        // Si falla se hace ROLLBACK y se relanza el error para que la capa de arriba lo muestre.
        public int Escribir(string query, SqlParameter[] parameters)
        {
            int fa = 0;
            IniciarTransaccion();
            try
            {
                fa = EscribirEnTransaccion(query, parameters);
                ConfirmarTransaccion();
            }
            catch (Exception)
            {
                CancelarTransaccion();
                throw;
            }
            return fa;
        }

        // ---- Transacciones con varias operaciones (ej: transferencia) ----
        // Uso: IniciarTransaccion() -> EscribirEnTransaccion(...) x N
        //      -> ConfirmarTransaccion() si todo OK, CancelarTransaccion() si algo falla.

        public void IniciarTransaccion()
        {
            Conectar();
            tr = cn.BeginTransaction();
        }

        public int EscribirEnTransaccion(string storedProcedure, SqlParameter[] parameters)
        {
            cmd = cn.CreateCommand();
            cmd.Connection = cn;
            cmd.Transaction = tr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedure;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd.ExecuteNonQuery();
        }

        public void ConfirmarTransaccion()
        {
            try
            {
                tr.Commit();
            }
            finally
            {
                tr = null;
                Desconectar();
            }
        }

        public void CancelarTransaccion()
        {
            try
            {
                if (tr != null)
                    tr.Rollback();
            }
            catch (Exception)
            {
                // Si el servidor ya deshizo la transacción, Rollback falla;
                // se ignora para que llegue el error original.
            }
            finally
            {
                tr = null;
                Desconectar();
            }
        }
    }
}
