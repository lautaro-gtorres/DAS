using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    // Reportes: los calculan los SP (ReporteVentas, ReporteCompras, ReporteProduccion);
    // acá solo se pasan a objetos BE.LineaReporte.
    public class MapperReporte
    {
        public List<BE.LineaReporte> Ventas(bool ultimoAnio)
        {
            return Ejecutar("ReporteVentas", ultimoAnio);
        }

        public List<BE.LineaReporte> Compras(bool ultimoAnio)
        {
            return Ejecutar("ReporteCompras", ultimoAnio);
        }

        public List<BE.LineaReporte> Produccion(bool ultimoAnio)
        {
            return Ejecutar("ReporteProduccion", ultimoAnio);
        }

        List<BE.LineaReporte> Ejecutar(string storedProcedure, bool ultimoAnio)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@ultimoAnio", ultimoAnio);
            DataTable dt = Acceso.Instancia.Leer(storedProcedure, parametros);

            List<BE.LineaReporte> lineas = new List<BE.LineaReporte>();
            foreach (DataRow row in dt.Rows)
            {
                BE.LineaReporte linea = new BE.LineaReporte(row["Concepto"].ToString());
                linea.Registros = Convert.ToInt32(row["Registros"]);
                linea.Cantidad = Convert.ToDecimal(row["Cantidad"]);
                linea.Valor = Convert.ToDecimal(row["Valor"]);
                lineas.Add(linea);
            }
            return lineas;
        }
    }
}
