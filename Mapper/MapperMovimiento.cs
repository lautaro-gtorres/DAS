using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    // Convierte las filas de ListarMovimientosPorUsuario en entidades BE.Movimiento.
    public class MapperMovimiento
    {
        public BE.Movimiento Mapear(DataRow row)
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
            if (row.Table.Columns.Contains("idUsuario"))
                movimiento.IdUsuario = Convert.ToInt32(row["idUsuario"]);
            return movimiento;
        }

        public List<BE.Movimiento> MapearLista(DataTable tabla)
        {
            List<BE.Movimiento> movimientos = new List<BE.Movimiento>();
            foreach (DataRow row in tabla.Rows)
            {
                movimientos.Add(Mapear(row));
            }
            return movimientos;
        }
    }
}
