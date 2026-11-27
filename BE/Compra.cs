using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Compra: compra de alimento (suministro) a un proveedor, en kilogramos.
    /// </summary>
    public class Compra : Movimiento
    {
        public Compra()
        {
        }

        public Compra(int idAlimento, int idProveedor, decimal kilogramos, decimal precioUnitario)
        {
            this.idAlimento = idAlimento;
            this.idProveedor = idProveedor;
            this.kilogramos = kilogramos;
            PrecioUnitario = precioUnitario;
        }

        public override string TipoMovimiento()
        {
            return "Compra";
        }

        // Ej: "500,00 kg de Maíz a Agro SRL".
        public override string Detalle()
        {
            return Kilogramos.ToString("N2") + " kg de " + Alimento + " a " + Proveedor;
        }

        // Una compra sale plata.
        public override decimal Importe()
        {
            return -Total;
        }

        private int idCompra;
        public int IdCompra
        {
            get { return idCompra; }
            set { idCompra = value; }
        }


        private int idAlimento;
        public int IdAlimento
        {
            get { return idAlimento; }
            set { idAlimento = value; }
        }

        // Nombre del alimento (lo completa el mapper con un JOIN).
        private string alimento;
        public string Alimento
        {
            get { return alimento; }
            set { alimento = value; }
        }

        private int idProveedor;
        public int IdProveedor
        {
            get { return idProveedor; }
            set { idProveedor = value; }
        }

        // Nombre del proveedor (lo completa el mapper con un JOIN).
        private string proveedor;
        public string Proveedor
        {
            get { return proveedor; }
            set { proveedor = value; }
        }

        private decimal kilogramos;
        public decimal Kilogramos
        {
            get { return kilogramos; }
            set { kilogramos = value; }
        }


    }
}
