using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de proveedores.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLProveedor : IABMeable<BE.Proveedor>
    {
        MapperProveedor mapper = new MapperProveedor();

        public int Alta(BE.Proveedor proveedor)
        {
            Validar(proveedor);
            try
            {
                return mapper.Alta(proveedor);
            }
            catch (SqlException ex)
            {
                TraducirError(ex, proveedor);
                throw;
            }
        }

        // 2601 = CUIL repetido (índice único).
        void TraducirError(SqlException ex, BE.Proveedor proveedor)
        {
            if (ex.Number == 2601)
                throw new ArgumentException("Ya hay un proveedor con el CUIL " + proveedor.Cuil + ".");
        }

        public int Baja(int id)
        {
            try
            {
                return mapper.Baja(id);
            }
            catch (SqlException ex)
            {
                // 547 = el proveedor está siendo usado por otra tabla (FK).
                if (ex.Number == 547)
                    throw new ArgumentException("No se puede eliminar el proveedor porque tiene alimentos cargados. Puede ponerlo como INACTIVO.");
                throw;
            }
        }

        public int Modificar(BE.Proveedor proveedor)
        {
            Validar(proveedor);
            try
            {
                return mapper.Modificar(proveedor);
            }
            catch (SqlException ex)
            {
                TraducirError(ex, proveedor);
                throw;
            }
        }

        // Activos e inactivos (para el ABM, que muestra el estado).
        public List<BE.Proveedor> Listar()
        {
            return mapper.Listar();
        }

        // Solo los activos (para elegir proveedor, por ejemplo al cargar un alimento).
        public List<BE.Proveedor> ListarActivos()
        {
            List<BE.Proveedor> activos = new List<BE.Proveedor>();
            foreach (BE.Proveedor proveedor in mapper.Listar())
            {
                if (proveedor.Activo)
                    activos.Add(proveedor);
            }
            return activos;
        }

        public List<BE.Proveedor> Buscar(BE.Proveedor filtro)
        {
            return mapper.Buscar(filtro);
        }

        void Validar(BE.Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentException("No hay datos del proveedor.");

            if (proveedor.Nombre == null)
                proveedor.Nombre = "";
            proveedor.Nombre = proveedor.Nombre.Trim();
            if (proveedor.Nombre == "")
                throw new ArgumentException("Ingrese el nombre del proveedor.");
            if (proveedor.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede tener más de 100 caracteres.");

            // El resto es opcional: si viene vacío se guarda como NULL.
            proveedor.Cuil = Validaciones.SacarEspacios(proveedor.Cuil);
            if (proveedor.Cuil != null)
                ValidarCuil(proveedor.Cuil);

            proveedor.Direccion = Validaciones.SacarEspacios(proveedor.Direccion);
            if (proveedor.Direccion != null && proveedor.Direccion.Length > 150)
                throw new ArgumentException("La dirección no puede tener más de 150 caracteres.");

            proveedor.Localidad = Validaciones.SacarEspacios(proveedor.Localidad);
            if (proveedor.Localidad != null && proveedor.Localidad.Length > 50)
                throw new ArgumentException("La localidad no puede tener más de 50 caracteres.");

            proveedor.Telefono = Validaciones.SacarEspacios(proveedor.Telefono);
            if (proveedor.Telefono != null && proveedor.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");

            proveedor.Email = Validaciones.SacarEspacios(proveedor.Email);
            if (proveedor.Email != null)
            {
                if (proveedor.Email.Length > 100)
                    throw new ArgumentException("El e-mail no puede tener más de 100 caracteres.");
                if (!Validaciones.EsEmailValido(proveedor.Email))
                    throw new ArgumentException("El e-mail no es válido.");
            }

            proveedor.ServicioProducto = Validaciones.SacarEspacios(proveedor.ServicioProducto);
            if (proveedor.ServicioProducto != null && proveedor.ServicioProducto.Length > 100)
                throw new ArgumentException("El servicio/producto no puede tener más de 100 caracteres.");
        }

        // El CUIL tiene 11 números; se acepta con o sin guiones (20-12345678-9 o 20123456789).
        void ValidarCuil(string cuil)
        {
            int numeros = 0;
            foreach (char c in cuil)
            {
                if (char.IsDigit(c))
                    numeros++;
                else if (c != '-')
                    throw new ArgumentException("El CUIL solo puede tener números y guiones.");
            }
            if (numeros != 11 || cuil.Length > 13)
                throw new ArgumentException("El CUIL tiene que tener 11 números (ej: 20-12345678-9).");
        }
    }
}
