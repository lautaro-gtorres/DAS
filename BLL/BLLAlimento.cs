using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de alimentos.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLAlimento : IABMeable<BE.Alimento>
    {
        MapperAlimento mapper = new MapperAlimento();

        public int Alta(BE.Alimento alimento)
        {
            Validar(alimento);
            try
            {
                return mapper.Alta(alimento);
            }
            catch (SqlException ex)
            {
                // 547 = el proveedor no existe (FK).
                if (ex.Number == 547)
                    throw new ArgumentException("El proveedor seleccionado no existe.");
                throw;
            }
        }

        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Alimento alimento)
        {
            Validar(alimento);
            try
            {
                return mapper.Modificar(alimento);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    throw new ArgumentException("El proveedor seleccionado no existe.");
                throw;
            }
        }

        public List<BE.Alimento> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Alimento> Buscar(BE.Alimento filtro)
        {
            return mapper.Buscar(filtro);
        }

        void Validar(BE.Alimento alimento)
        {
            if (alimento == null)
                throw new ArgumentException("No hay datos del alimento.");

            if (alimento.NombreAlimento == null)
                alimento.NombreAlimento = "";
            alimento.NombreAlimento = alimento.NombreAlimento.Trim();
            if (alimento.NombreAlimento == "")
                throw new ArgumentException("Ingrese el nombre del alimento.");
            if (alimento.NombreAlimento.Length > 100)
                throw new ArgumentException("El nombre no puede tener más de 100 caracteres.");
            if (alimento.CantidadDisponible < 0)
                throw new ArgumentException("La cantidad disponible no puede ser negativa.");
            if (alimento.CantidadDisponible > 99999999.99m)
                throw new ArgumentException("La cantidad disponible es demasiado grande.");
            if (alimento.IdProveedor <= 0)
                throw new ArgumentException("Seleccione el proveedor.");
        }
    }
}
