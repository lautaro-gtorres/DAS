using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Compras de alimento (suministros) a proveedores. Al comprar se suma al stock;
    // al anular la compra se resta.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLCompra : IABMeable<BE.Compra>
    {
        MapperCompra mapper = new MapperCompra();

        public int Alta(BE.Compra compra)
        {
            if (compra == null)
                throw new ArgumentException("No hay datos de la compra.");
            if (compra.IdAlimento <= 0)
                throw new ArgumentException("Seleccione el suministro.");
            if (compra.IdProveedor <= 0)
                throw new ArgumentException("Seleccione el proveedor.");
            if (compra.Kilogramos <= 0)
                throw new ArgumentException("La cantidad (en kilogramos) tiene que ser mayor a cero.");
            if (compra.Kilogramos > 99999999m)
                throw new ArgumentException("La cantidad es demasiado grande.");
            ValidarFechaPrecio(compra);
            try
            {
                return mapper.Alta(compra);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    throw new ArgumentException("El suministro o el proveedor seleccionados no existen.");
                if (ex.Number == 8115)
                    throw new ArgumentException("Con esta compra el stock del alimento supera el máximo permitido.");
                throw;
            }
        }

        // Anula la compra y resta los kilos del stock.
        public int Baja(int id)
        {
            try
            {
                return mapper.Baja(id);
            }
            catch (SqlException ex)
            {
                // 547 = el CHECK de Alimento no deja que el stock quede negativo.
                if (ex.Number == 547)
                    throw new ArgumentException("No se puede anular la compra: ese alimento ya se usó y el stock quedaría negativo.");
                throw;
            }
        }

        // Solo se corrigen la fecha y el precio (los kilos ya se sumaron al stock).
        public int Modificar(BE.Compra compra)
        {
            if (compra == null)
                throw new ArgumentException("No hay datos de la compra.");
            ValidarFechaPrecio(compra);
            return mapper.Modificar(compra);
        }

        public List<BE.Compra> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Compra> Buscar(BE.Compra filtro)
        {
            return mapper.Buscar(filtro);
        }

        void ValidarFechaPrecio(BE.Compra compra)
        {
            if (compra.Fecha == DateTime.MinValue)
                compra.Fecha = DateTime.Today;
            if (compra.Fecha.Date > DateTime.Today)
                throw new ArgumentException("La fecha de la compra no puede ser futura.");
            if (compra.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario tiene que ser mayor a cero.");
            if (compra.PrecioUnitario > 99999999m)
                throw new ArgumentException("El precio unitario es demasiado grande.");
        }
    }
}
