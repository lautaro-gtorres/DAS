using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Transferencia
    {
        Acceso acc = new Acceso();

        public const string TipoEgreso = "Egreso";
        public const string TipoIngreso = "Ingreso";

        // Ejecuta todos los pasos de la transferencia en UNA transacción:
        // registra la transferencia, descuenta al emisor, acredita al receptor
        // y registra los dos movimientos. COMMIT si todo sale bien, ROLLBACK si algo falla.
        public int Realizar(BE.Transferencia transferencia)
        {
            acc.IniciarTransaccion();
            try
            {
                // 0) Registrar la transferencia (devuelve el id generado)
                SqlParameter[] parametros = new SqlParameter[4];
                parametros[0] = new SqlParameter("@usuarioEmisor", transferencia.UsuarioEmisor);
                parametros[1] = new SqlParameter("@usuarioReceptor", transferencia.UsuarioReceptor);
                parametros[2] = new SqlParameter("@importe", transferencia.Importe);
                parametros[3] = new SqlParameter("@idTransferencia", SqlDbType.Int);
                parametros[3].Direction = ParameterDirection.Output;
                acc.EscribirEnTransaccion("AltaTransferencia", parametros);
                int idTransferencia = Convert.ToInt32(parametros[3].Value);

                // 1) Descontar el importe al emisor (0 filas = saldo insuficiente o inactivo)
                SqlParameter[] debito = new SqlParameter[2];
                debito[0] = new SqlParameter("@idUsuario", transferencia.UsuarioEmisor);
                debito[1] = new SqlParameter("@importe", transferencia.Importe);
                if (acc.EscribirEnTransaccion("DebitarSaldo", debito) == 0)
                    throw new InvalidOperationException("Saldo insuficiente. La transferencia se canceló.");

                // 2) Acreditar el importe al receptor (0 filas = no existe o inactivo)
                SqlParameter[] credito = new SqlParameter[2];
                credito[0] = new SqlParameter("@idUsuario", transferencia.UsuarioReceptor);
                credito[1] = new SqlParameter("@importe", transferencia.Importe);
                if (acc.EscribirEnTransaccion("AcreditarSaldo", credito) == 0)
                    throw new InvalidOperationException("El destinatario no está disponible. La transferencia se canceló.");

                // 3) Movimiento del emisor
                acc.EscribirEnTransaccion("AltaMovimiento", ParametrosMovimiento(
                    transferencia.UsuarioEmisor, TipoEgreso, transferencia.Importe,
                    transferencia.UsuarioReceptor, transferencia.Concepto, idTransferencia));

                // 4) Movimiento del receptor
                acc.EscribirEnTransaccion("AltaMovimiento", ParametrosMovimiento(
                    transferencia.UsuarioReceptor, TipoIngreso, transferencia.Importe,
                    transferencia.UsuarioEmisor, transferencia.Concepto, idTransferencia));

                acc.ConfirmarTransaccion();
                return idTransferencia;
            }
            catch (Exception)
            {
                acc.CancelarTransaccion();
                throw;
            }
        }

        SqlParameter[] ParametrosMovimiento(int idUsuario, string tipo, double importe,
            int usuarioRelacionado, string concepto, int idTransferencia)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("@idUsuario", idUsuario);
            parametros[1] = new SqlParameter("@tipo", tipo);
            parametros[2] = new SqlParameter("@importe", importe);
            parametros[3] = new SqlParameter("@usuarioRelacionado", usuarioRelacionado);
            parametros[4] = new SqlParameter("@concepto", concepto);
            parametros[5] = new SqlParameter("@idTransferencia", idTransferencia);
            return parametros;
        }
    }
}
