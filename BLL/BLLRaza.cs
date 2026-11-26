using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Reglas de negocio del ABM de razas.
    /// Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    /// </summary>
    public class BLLRaza : IABMeable<BE.Raza>
    {
        MapperRaza mapper = new MapperRaza();

        public int Alta(BE.Raza raza)
        {
            Validar(raza);
            return ErrorBD.Ejecutar(() => mapper.Alta(raza), "Ya existe la raza " + raza.NombreRaza + ".");
        }

        public int Baja(int id)
        {
            return ErrorBD.Ejecutar(() => mapper.Baja(id),
                mensajeEnUso: "No se puede eliminar la raza porque hay gallinas de esa raza.");
        }

        public int Modificar(BE.Raza raza)
        {
            Validar(raza);
            return ErrorBD.Ejecutar(() => mapper.Modificar(raza), "Ya existe la raza " + raza.NombreRaza + ".");
        }

        public List<BE.Raza> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Raza> Buscar(BE.Raza filtro)
        {
            return mapper.Buscar(filtro);
        }

        void Validar(BE.Raza raza)
        {
            if (raza == null)
                throw new ArgumentException("No hay datos de la raza.");

            raza.NombreRaza = (raza.NombreRaza ?? "").Trim();
            if (raza.NombreRaza == "")
                throw new ArgumentException("Ingrese el nombre de la raza.");
            if (raza.NombreRaza.Length > 50)
                throw new ArgumentException("El nombre de la raza no puede tener más de 50 caracteres.");
        }
    }
}
