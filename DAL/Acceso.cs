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
    // Patrón Singleton: hay un solo objeto Acceso en toda la aplicación.
    // Se usa con Acceso.Instancia (el constructor es privado, no se puede hacer "new Acceso()").
    public class Acceso
    {
        static Acceso instancia;

        public static Acceso Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new Acceso();
                return instancia;
            }
        }

        private Acceso()
        {
        }

        SqlConnection cn = new SqlConnection();
        SqlCommand cmd;
        SqlTransaction tr;

        public void Conectar()
        {
            // La cadena de conexión está en ConnectionStrings.config (no se sube al repo).
            ConnectionStringSettings config = ConfigurationManager.ConnectionStrings["DBPollos"];
            if (config == null)
                throw new Exception("Falta la cadena de conexión 'DBPollos'. Copiá ConnectionStrings.example.config como ConnectionStrings.config y completalo.");

            // Se da más tiempo para conectar que los 15 segundos por defecto.
            SqlConnectionStringBuilder datos = new SqlConnectionStringBuilder(config.ConnectionString);
            if (datos.ConnectTimeout < 30)
                datos.ConnectTimeout = 30;
            cn.ConnectionString = datos.ConnectionString;

            // Si la base de Azure estuvo sin uso se pausa sola y, mientras vuelve a arrancar,
            // las conexiones fallan. Se reintenta hasta 4 veces, esperando 5 segundos entre intentos.
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

        // Una sola escritura dentro de su propia transacción.
        // Si falla se hace ROLLBACK y se relanza el error para que la capa de arriba lo muestre.
        public int Escribir(string storedProcedure, SqlParameter[] parameters)
        {
            int fa = 0;
            IniciarTransaccion();
            try
            {
                fa = EscribirEnTransaccion(storedProcedure, parameters);
                ConfirmarTransaccion();
            }
            catch (Exception)
            {
                CancelarTransaccion();
                throw;
            }
            return fa;
        }

        // ---- Transacciones con varias operaciones (ej: una venta con sus detalles) ----
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

        // ---- XML ----

        // Llena un DataSet con el SP y lo guarda en el archivo XML.
        public void ExportarXML(string storedProcedure, string tabla, string ruta)
        {
            DataSet ds = new DataSet();
            try
            {
                Conectar();
                SqlDataAdapter da = new SqlDataAdapter(storedProcedure, cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(ds, tabla);
            }
            finally
            {
                Desconectar();
            }
            ds.WriteXml(ruta);
        }

        // Igual que el anterior, para un SP con parámetros (ej: el comprobante de una venta).
        public void ExportarXML(string storedProcedure, SqlParameter[] parameters, string tabla, string ruta)
        {
            DataSet ds = new DataSet();
            try
            {
                Conectar();
                SqlDataAdapter da = new SqlDataAdapter(storedProcedure, cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    da.SelectCommand.Parameters.AddRange(parameters);
                }
                da.Fill(ds, tabla);
            }
            finally
            {
                Desconectar();
            }
            ds.WriteXml(ruta);
        }

        // Lee el archivo XML y lo devuelve en un DataSet.
        public DataSet LeerXML(string ruta)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(ruta);
            return ds;
        }
    }
}
