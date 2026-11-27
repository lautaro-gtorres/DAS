using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Venta: una venta es una sola línea (huevos o gallinas).
    /// </summary>
    public class Venta : Movimiento
    {
        public Venta()
        {
        }

        // Venta de huevos.
        public Venta(int cantidadHuevos, decimal precioUnitario)
        {
            IdTipoProducto = 1;
            Cantidad = cantidadHuevos;
            PrecioUnitario = precioUnitario;
        }

        public override string TipoMovimiento()
        {
            return "Venta";
        }

        // Ej: "30 huevos" o "2 gallinas Ponedora Sussex (H)", con el cliente si lo hay.
        public override string Detalle()
        {
            string detalle;
            if (IdTipoGallina == null)
                detalle = Cantidad + " huevos";
            else
                detalle = Cantidad + " gallinas " + TipoGallina + " " + Raza + " (" + Sexo + ")";
            if (Cliente != null)
                detalle = detalle + " a " + Cliente;
            return detalle;
        }

        // Una venta entra plata.
        public override decimal Importe()
        {
            return Total;
        }

        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }


        // 1 = Huevo, 2 = Gallina (tabla TipoProducto).
        private int idTipoProducto;
        public int IdTipoProducto
        {
            get { return idTipoProducto; }
            set { idTipoProducto = value; }
        }

        // Dato de la tabla relacionada (lo completa el mapper con un JOIN, para mostrar en listados).
        // Huevo o Gallina.
        private string tipoProducto;
        public string TipoProducto
        {
            get { return tipoProducto; }
            set { tipoProducto = value; }
        }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }



        // El cliente es opcional.
        private int? idCliente;
        public int? IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }

        // "Apellido, Nombre" del cliente (lo completa el mapper con un JOIN).
        private string cliente;
        public string Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        // ---- Solo en ventas de gallinas (en ventas de huevos quedan en null) ----

        private int? idTipoGallina;
        public int? IdTipoGallina
        {
            get { return idTipoGallina; }
            set { idTipoGallina = value; }
        }

        private string tipoGallina;
        public string TipoGallina
        {
            get { return tipoGallina; }
            set { tipoGallina = value; }
        }

        private int? idRaza;
        public int? IdRaza
        {
            get { return idRaza; }
            set { idRaza = value; }
        }

        private string raza;
        public string Raza
        {
            get { return raza; }
            set { raza = value; }
        }

        // 'M' = macho, 'H' = hembra.
        private char? sexo;
        public char? Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }
    }
}
