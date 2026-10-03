using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    // Convierte las filas de ListarTransferencias en entidades BE.Transferencia.
    public class MapperTransferencia
    {
        public BE.Transferencia Mapear(DataRow row)
        {
            BE.Transferencia transferencia = new BE.Transferencia();
            transferencia.IdTransferencia = Convert.ToInt32(row["idTransferencia"]);
            transferencia.UsuarioEmisor = Convert.ToInt32(row["usuarioEmisor"]);
            transferencia.UsuarioReceptor = Convert.ToInt32(row["usuarioReceptor"]);
            transferencia.Importe = Convert.ToDouble(row["importe"]);
            transferencia.FechaHora = Convert.ToDateTime(row["fechaHora"]);
            return transferencia;
        }

        public List<BE.Transferencia> MapearLista(DataTable tabla)
        {
            List<BE.Transferencia> transferencias = new List<BE.Transferencia>();
            foreach (DataRow row in tabla.Rows)
            {
                transferencias.Add(Mapear(row));
            }
            return transferencias;
        }
    }
}
