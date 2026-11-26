using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// Punto único de acceso a la base de datos (patrón Singleton).
    /// Toda la aplicación comparte esta instancia y su conexión:
    /// se obtiene con Acceso.Instancia y no se puede crear con "new".
    /// </summary>
    public sealed class Acceso
    {
        // Lazy crea la instancia la primera vez que se usa y es seguro entre hilos.
        private static readonly Lazy<Acceso> instancia = new Lazy<Acceso>(() => new Acceso());

        public static Acceso Instancia
        {
            get { return instancia.Value; }
        }

        private readonly SqlConnection cn;
        private SqlTransaction tr;

        // Constructor privado: nadie fuera de esta clase puede crear otra instancia.
        private Acceso()
        {
            ConnectionStringSettings config = ConfigurationManager.ConnectionStrings["DBPollos"];
            if (config == null || string.IsNullOrWhiteSpace(config.ConnectionString))
                throw new InvalidOperationException(
                    "Falta la cadena de conexión 'DBPollos'. Copiá ConnectionStrings.example.config " +
                    "como ConnectionStrings.config y completalo.");

            // Se da más tiempo para conectar que los 15 segundos por defecto.
            SqlConnectionStringBuilder datos = new SqlConnectionStringBuilder(config.ConnectionString);
            if (datos.ConnectTimeout < 30)
                datos.ConnectTimeout = 30;

            cn = new SqlConnection(datos.ConnectionString);
        }

        private void Conectar()
        {
            if (cn.State == ConnectionState.Open)
                return;

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
        private bool EsErrorTransitorio(SqlException ex)
        {
            return ex.Number != 18456 && ex.Number != 40615;
        }

        private void Desconectar()
        {
            // Solo se cierra (no se hace Dispose): la conexión es única y se reutiliza.
            if (tr == null && cn.State != ConnectionState.Closed)
                cn.Close();
        }

        private SqlCommand CrearComando(string storedProcedure, SqlParameter[] parametros)
        {
            SqlCommand cmd = cn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcedure;
            cmd.Transaction = tr;
            if (parametros != null)
                cmd.Parameters.AddRange(parametros);
            return cmd;
        }

        public DataTable Leer(string storedProcedure, SqlParameter[] parametros = null)
        {
            DataTable dt = new DataTable();
            try
            {
                Conectar();
                using (SqlCommand cmd = CrearComando(storedProcedure, parametros))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
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
        public int Escribir(string storedProcedure, SqlParameter[] parametros = null)
        {
            int fa;
            IniciarTransaccion();
            try
            {
                fa = EscribirEnTransaccion(storedProcedure, parametros);
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
            if (tr != null)
                throw new InvalidOperationException("Ya hay una transacción en curso.");
            Conectar();
            tr = cn.BeginTransaction();
        }

        public int EscribirEnTransaccion(string storedProcedure, SqlParameter[] parametros = null)
        {
            if (tr == null)
                throw new InvalidOperationException("No hay una transacción iniciada.");
            using (SqlCommand cmd = CrearComando(storedProcedure, parametros))
            {
                return cmd.ExecuteNonQuery();
            }
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
