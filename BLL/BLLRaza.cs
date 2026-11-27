using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de razas.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLRaza : IABMeable<BE.Raza>
    {
        MapperRaza mapper = new MapperRaza();

        public int Alta(BE.Raza raza)
        {
            Validar(raza);
            try
            {
                return mapper.Alta(raza);
            }
            catch (SqlException ex)
            {
                // 2627 = dato repetido (el nombre de la raza es único).
                if (ex.Number == 2627)
                    throw new ArgumentException("Ya existe la raza " + raza.NombreRaza + ".");
                throw;
            }
        }

        public int Baja(int id)
        {
            try
            {
                return mapper.Baja(id);
            }
            catch (SqlException ex)
            {
                // 547 = la raza está siendo usada por otra tabla (FK).
                if (ex.Number == 547)
                    throw new ArgumentException("No se puede eliminar la raza porque hay gallinas de esa raza.");
                throw;
            }
        }

        public int Modificar(BE.Raza raza)
        {
            Validar(raza);
            try
            {
                return mapper.Modificar(raza);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new ArgumentException("Ya existe la raza " + raza.NombreRaza + ".");
                throw;
            }
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

            if (raza.NombreRaza == null)
                raza.NombreRaza = "";
            raza.NombreRaza = raza.NombreRaza.Trim();
            if (raza.NombreRaza == "")
                throw new ArgumentException("Ingrese el nombre de la raza.");
            if (raza.NombreRaza.Length > 50)
                throw new ArgumentException("El nombre de la raza no puede tener más de 50 caracteres.");
        }
    }
}
