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
            // Se da más tiempo para conectar que los 15 segundos por defecto.
            SqlConnectionStringBuilder datos = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString);
            if (datos.ConnectTimeout < 30)
                datos.ConnectTimeout = 30;
            cn.ConnectionString = datos.ConnectionString;

            // La base de Azure es "serverless": si no se usa por un rato se pausa sola y,
            // mientras vuelve a arrancar, las conexiones fallan con distintos errores
            // ("is not currently available", timeouts en el handshake, etc.).
            // Se reintenta hasta 4 veces, esperando 5 segundos entre intentos.
            int intento = 1;
            while (true)
            {
                try
                {
                    cn.Open();
                    return;
                }
                catch (SqlException ex) when (EsErrorTransitorio(ex) && intento < 4)
                {
                    intento++;
                    cn.Close();
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        // Errores que NO se arreglan reintentando: usuario o contraseña de la base
        // incorrectos (18456) o IP bloqueada por el firewall de Azure (40615).
        bool EsErrorTransitorio(SqlException ex)
        {
            return ex.Number != 18456 && ex.Number != 40615;
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
