using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Registros: ventas y compras juntas en una sola lista polimórfica (List<BE.Movimiento>).
    public class BLLMovimiento
    {
        MapperVenta mapperVenta = new MapperVenta();
        MapperCompra mapperCompra = new MapperCompra();

        // Todos los movimientos, del más nuevo al más viejo.
        public List<BE.Movimiento> Listar()
        {
            List<BE.Movimiento> movimientos = new List<BE.Movimiento>();
            foreach (BE.Venta venta in mapperVenta.Listar())
                Agregar(movimientos, venta);
            foreach (BE.Compra compra in mapperCompra.Listar())
                Agregar(movimientos, compra);
            return movimientos;
        }

        // Lo que entró menos lo que salió. Cada movimiento sabe su signo (Importe redefinido en Venta y Compra).
        public decimal Balance(List<BE.Movimiento> movimientos)
        {
            decimal balance = 0;
            foreach (BE.Movimiento movimiento in movimientos)
                balance = balance + movimiento.Importe();
            return balance;
        }

        // Inserta el movimiento manteniendo el orden por fecha (más nuevo primero).
        void Agregar(List<BE.Movimiento> movimientos, BE.Movimiento nuevo)
        {
            int posicion = 0;
            while (posicion < movimientos.Count && movimientos[posicion].Fecha >= nuevo.Fecha)
                posicion++;
            movimientos.Insert(posicion, nuevo);
        }
    }
}
