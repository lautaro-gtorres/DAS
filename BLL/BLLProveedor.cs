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
            return mapper.Alta(proveedor);
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
                    throw new ArgumentException("No se puede eliminar el proveedor porque tiene alimentos cargados.");
                throw;
            }
        }

        public int Modificar(BE.Proveedor proveedor)
        {
            Validar(proveedor);
            return mapper.Modificar(proveedor);
        }

        public List<BE.Proveedor> Listar()
        {
            return mapper.Listar();
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

            // Teléfono y dirección son opcionales: si vienen vacíos se guardan como NULL.
            proveedor.Telefono = SacarEspacios(proveedor.Telefono);
            if (proveedor.Telefono != null && proveedor.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");
            proveedor.Direccion = SacarEspacios(proveedor.Direccion);
            if (proveedor.Direccion != null && proveedor.Direccion.Length > 150)
                throw new ArgumentException("La dirección no puede tener más de 150 caracteres.");
        }

        // Devuelve el texto sin espacios de los costados, o null si está vacío.
        string SacarEspacios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;
            return texto.Trim();
        }
    }
}
