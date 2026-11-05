using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapPago
    {
        Acceso acc = new Acceso();
        public int AltaPago(BE.Pago pago)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[5];
            //parametro[0] = new SqlParameter("@IdLote", lote.IdLote);
            parametro[0] = new SqlParameter("@nroLote", pago.NroLote);
            parametro[1] = new SqlParameter("@ubicacion", pago.Ubicacion);
            parametro[2] = new SqlParameter("@mts2Total", pago.Mt2Total);
            parametro[3] = new SqlParameter("@precioMt2", pago.PrecioMt2);
            parametro[4] = new SqlParameter("@total", pago.Total);

            fa = acc.Escribir("AltaPago", parametro);
            return fa;
        }

        public List<string> ListaPagados()
        {
            List<string> lotes = new List<string>();
            var dt = acc.Leer("ListaPagados");
            foreach (System.Data.DataRow row in dt.Rows)
            {
               
               
                lotes.Add(row["nroLote"].ToString());
            }
            return lotes;
        }

        public string PromedioTotal()
        {
            var dt = acc.Leer("PromedioTotal");
            return dt.Rows[0][0].ToString();
        }
    }
}
