using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Movimiento de dinero de la granja. Clase base de Venta (entra plata) y Compra (sale plata).
    /// Con una List&lt;Movimiento&gt; se muestran ventas y compras juntas (Registros).
    /// </summary>
    public abstract class Movimiento
    {
        // Solo las clases derivadas pueden crear un Movimiento.
        protected Movimiento()
        {
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private decimal precioUnitario;
        public decimal PrecioUnitario
        {
            get { return precioUnitario; }
            set { precioUnitario = value; }
        }

        // Columna calculada en la base (cantidad * precio unitario): no se envía al guardar.
        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        // "Venta" o "Compra".
        public abstract string TipoMovimiento();

        // Descripción legible del movimiento (cada clase arma la suya).
        public abstract string Detalle();

        // Importe con signo: positivo si entra plata, negativo si sale.
        public abstract decimal Importe();
    }
}
