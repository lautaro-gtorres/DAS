using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    // Los movimientos solo se crean dentro de la transferencia (DAL.Transferencia),
    // acá solo se consultan.
    public class Movimiento
    {
        Acceso acc = new Acceso();
        Mapper.MapperMovimiento mapper = new Mapper.MapperMovimiento();

        public List<BE.Movimiento> ListarPorUsuario(int idUsuario)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idUsuario", idUsuario);
            DataTable dt = acc.Leer("ListarMovimientosPorUsuario", parametros);
            List<BE.Movimiento> movimientos = mapper.MapearLista(dt);
            foreach (BE.Movimiento movimiento in movimientos)
            {
                movimiento.IdUsuario = idUsuario;
            }
            return movimientos;
        }
    }
}
