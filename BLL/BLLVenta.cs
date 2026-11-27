using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio de las ventas (huevos y gallinas).
    // Vender gallinas las da de baja; anular la venta las vuelve a activar.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLVenta : IABMeable<BE.Venta>
    {
        // Ids de la tabla TipoProducto.
        public const int ProductoHuevo = 1;
        public const int ProductoGallina = 2;

        MapperVenta mapper = new MapperVenta();

        public int Alta(BE.Venta venta)
        {
            Validar(venta);

            // Se avisa antes de intentar si no alcanzan las gallinas (el SP lo vuelve a controlar).
            if (venta.IdTipoProducto == ProductoGallina)
            {
                int disponibles = mapper.ContarGallinasDisponibles(venta.IdTipoGallina.Value, venta.IdRaza.Value, venta.Sexo.Value);
                if (disponibles < venta.Cantidad)
                    throw new ArgumentException("No hay suficientes gallinas de ese tipo, raza y sexo: hay " + disponibles + " disponibles.");
            }
            else
            {
                int huevos = mapper.StockHuevos();
                if (huevos < venta.Cantidad)
                    throw new ArgumentException("No hay suficientes huevos: hay " + huevos + " disponibles.");
            }

            try
            {
                return mapper.Alta(venta);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
                throw;
            }
        }

        // Anula la venta: las gallinas vendidas vuelven a estar activas.
        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        // Solo se pueden corregir la fecha, el cliente y el precio unitario.
        // Para cambiar el producto o la cantidad hay que anular la venta y cargarla de nuevo.
        public int Modificar(BE.Venta venta)
        {
            if (venta == null)
                throw new ArgumentException("No hay datos de la venta.");
            ValidarFechaClientePrecio(venta);
            try
            {
                return mapper.Modificar(venta);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
                throw;
            }
        }

        public List<BE.Venta> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Venta> Buscar(BE.Venta filtro)
        {
            return mapper.Buscar(filtro);
        }

        public List<BE.TipoProducto> ListarTiposProducto()
        {
            return mapper.ListarTiposProducto();
        }

        public int ContarGallinasDisponibles(int idTipoGallina, int idRaza, char sexo)
        {
            return mapper.ContarGallinasDisponibles(idTipoGallina, idRaza, char.ToUpper(sexo));
        }

        // Huevos disponibles = producidos - vendidos.
        public int StockHuevos()
        {
            return mapper.StockHuevos();
        }

        // 50001 / 50002 = el SP no encontró suficientes gallinas / huevos;
        // 547 = cliente, tipo o raza inexistente (FK).
        void TraducirError(SqlException ex)
        {
            if (ex.Number == 50001 || ex.Number == 50002)
                throw new ArgumentException(ex.Message);
            if (ex.Number == 547)
                throw new ArgumentException("El cliente, el tipo o la raza seleccionados no existen.");
        }

        void Validar(BE.Venta venta)
        {
            if (venta == null)
                throw new ArgumentException("No hay datos de la venta.");

            if (venta.IdTipoProducto != ProductoHuevo && venta.IdTipoProducto != ProductoGallina)
                throw new ArgumentException("Seleccione qué se vende: huevos o gallinas.");
            if (venta.Cantidad <= 0)
                throw new ArgumentException("La cantidad tiene que ser mayor a cero.");

            if (venta.IdTipoProducto == ProductoGallina)
            {
                if (venta.IdTipoGallina == null || venta.IdTipoGallina <= 0)
                    throw new ArgumentException("Seleccione el tipo de gallina.");
                if (venta.IdRaza == null || venta.IdRaza <= 0)
                    throw new ArgumentException("Seleccione la raza de gallina.");
                if (venta.Sexo == null)
                    throw new ArgumentException("Seleccione el sexo de gallina.");
                venta.Sexo = char.ToUpper(venta.Sexo.Value);
                if (venta.Sexo != BE.Gallina.Macho && venta.Sexo != BE.Gallina.Hembra)
                    throw new ArgumentException("Seleccione el sexo (M = macho, H = hembra).");
            }
            else
            {
                // En una venta de huevos no se guardan datos de gallina.
                venta.IdTipoGallina = null;
                venta.IdRaza = null;
                venta.Sexo = null;
            }

            ValidarFechaClientePrecio(venta);
        }

        void ValidarFechaClientePrecio(BE.Venta venta)
        {
            // Si no se indica fecha, la venta es de hoy.
            if (venta.Fecha == DateTime.MinValue)
                venta.Fecha = DateTime.Today;
            if (venta.Fecha.Date > DateTime.Today)
                throw new ArgumentException("La fecha de la venta no puede ser futura.");

            // El cliente es opcional.
            if (venta.IdCliente != null && venta.IdCliente <= 0)
                venta.IdCliente = null;

            if (venta.PrecioUnitario <= 0)
                throw new ArgumentException("El precio unitario tiene que ser mayor a cero.");
            if (venta.PrecioUnitario > 99999999m)
                throw new ArgumentException("El precio unitario es demasiado grande.");
        }
    }
}
