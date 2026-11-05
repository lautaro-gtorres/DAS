using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapLotes
    {
        Acceso acc = new Acceso();
        public int AltaLote(BE.Lotes lote)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[4];
            //parametro[0] = new SqlParameter("@IdLote", lote.IdLote);
            parametro[0] = new SqlParameter("@nombrePropietario", lote.NombrePropietario);
            parametro[1] = new SqlParameter("@medidaFrente", lote.MedidaFrente);
            parametro[2] = new SqlParameter("@medidaFondo", lote.MedidaFondo);
            parametro[3] = new SqlParameter("@totalMt2", lote.TotalMts2);

            fa = acc.Escribir("AltaLote", parametro);
            return fa;
        }
        public int BajaLote(int idLote)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter("@IdLote", idLote);
            fa = acc.Escribir("BajaLote", parametro);
            return fa;
        }

        public List<BE.Lotes> ListarLotes()
        {
            List<BE.Lotes> lotes = new List<BE.Lotes>();
            var dt = acc.Leer("ListarLotes");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Lotes lote = new BE.Lotes(
                    Convert.ToInt32(row["NroLote"]),
                    row["NombrePropietario"].ToString(),
                    Convert.ToInt32(row["MedidaFrente"]),
                    Convert.ToInt32(row["MedidaFondo"]),
                    Convert.ToInt32(row["TotalMts2"])
                );
                lotes.Add(lote);
            }
            return lotes;
        }
        public int ModificarLote(BE.Lotes lote)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[5];
            parametro[0] = new SqlParameter("@nroLote", lote.NroLote);
            parametro[1] = new SqlParameter("@nombrePropietario", lote.NombrePropietario);
            parametro[2] = new SqlParameter("@medidaFrente", lote.MedidaFrente);
            parametro[3] = new SqlParameter("@medidaFondo", lote.MedidaFondo);
            parametro[4] = new SqlParameter("@totalMt2", lote.TotalMts2);
            fa = acc.Escribir("EditarLote", parametro);
            return fa;
        }
        public List<BE.Lotes> Buscar(BE.Lotes lote)
        {
            List<BE.Lotes> ls = new List<BE.Lotes>();

            List<SqlParameter> parametros = new List<SqlParameter>();

            
            return ls;



        }
    }
}
