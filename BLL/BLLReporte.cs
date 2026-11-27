using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reportes del último mes o del último año: ventas totales, compras de alimento
    // y producción promedio de huevos. Todo el cálculo (período, agrupación, totales
    // y promedios) lo hacen los stored procedures de 09_Reportes.sql.
    public class BLLReporte
    {
        // Concepto de la última fila de cada reporte.
        public const string Total = "TOTAL";

        MapperReporte mapper = new MapperReporte();

        // Ventas por producto: cantidad de ventas, unidades y pesos.
        public List<BE.LineaReporte> Ventas(bool ultimoAnio)
        {
            return mapper.Ventas(ultimoAnio);
        }

        // Compras por alimento: cantidad de compras, kilos y pesos.
        public List<BE.LineaReporte> Compras(bool ultimoAnio)
        {
            return mapper.Compras(ultimoAnio);
        }

        // Producción por corral: días con registro, huevos y promedio por día.
        public List<BE.LineaReporte> ProduccionPromedio(bool ultimoAnio)
        {
            return mapper.Produccion(ultimoAnio);
        }
    }
}
