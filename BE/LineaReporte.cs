using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Una fila de un reporte (Reportes): un concepto agrupado con sus totales.
    /// Según el reporte, el concepto es un producto, un alimento o un corral.
    /// </summary>
    public class LineaReporte
    {
        public LineaReporte()
        {
        }

        public LineaReporte(string concepto)
        {
            this.concepto = concepto;
        }

        // Producto, alimento o corral.
        private string concepto;
        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; }
        }

        // Cantidad de ventas, de compras o de días con producción.
        private int registros;
        public int Registros
        {
            get { return registros; }
            set { registros = value; }
        }

        // Unidades vendidas, kilos comprados o huevos producidos.
        private decimal cantidad;
        public decimal Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        // Importe en pesos (ventas y compras) o promedio de huevos por día (producción).
        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
    }
}
