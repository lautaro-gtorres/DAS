using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Reglas de negocio del ABM de proveedores.
    /// Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    /// </summary>
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
            return ErrorBD.Ejecutar(() => mapper.Baja(id),
                mensajeEnUso: "No se puede eliminar el proveedor porque tiene alimentos cargados.");
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

            proveedor.Nombre = (proveedor.Nombre ?? "").Trim();
            if (proveedor.Nombre == "")
                throw new ArgumentException("Ingrese el nombre del proveedor.");
            if (proveedor.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede tener más de 100 caracteres.");

            // Teléfono y dirección son opcionales: si vienen vacíos se guardan como NULL.
            proveedor.Telefono = Opcional(proveedor.Telefono);
            if (proveedor.Telefono != null && proveedor.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");
            proveedor.Direccion = Opcional(proveedor.Direccion);
            if (proveedor.Direccion != null && proveedor.Direccion.Length > 150)
                throw new ArgumentException("La dirección no puede tener más de 150 caracteres.");
        }

        static string Opcional(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        }
    }
}
