using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    // Los movimientos solo se crean dentro de la transferencia (MapperTransferencia),
    // acá solo se consultan. Por eso no implementa IMapper (no hay baja ni modificación).
    public class MapperMovimiento
    {
        DAL.Acceso acc = new DAL.Acceso();

        public List<BE.Movimiento> ListarPorUsuario(int idUsuario)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idUsuario", idUsuario);
            DataTable dt = acc.Leer("ListarMovimientosPorUsuario", parametros);

            List<BE.Movimiento> movimientos = new List<BE.Movimiento>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Movimiento movimiento = Mapear(row);
                movimiento.IdUsuario = idUsuario;
                movimientos.Add(movimiento);
            }
            return movimientos;
        }

        // ---- Fila de la base -> entidad BE ----

        BE.Movimiento Mapear(DataRow row)
        {
            BE.Movimiento movimiento = new BE.Movimiento();
            movimiento.IdMovimiento = Convert.ToInt32(row["idMovimiento"]);
            movimiento.FechaHora = Convert.ToDateTime(row["fechaHora"]);
            movimiento.Tipo = row["tipo"].ToString();
            movimiento.Importe = Convert.ToDouble(row["importe"]);
            movimiento.IdUsuarioRelacionado = Convert.ToInt32(row["usuarioRelacionado"]);
            movimiento.UsuarioRelacionado = row["usuarioRelacionadoNombre"].ToString();
            movimiento.Concepto = row["concepto"].ToString();
            movimiento.IdTransferencia = Convert.ToInt32(row["idTransferencia"]);
            return movimiento;
        }
    }
}
