using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de corrales.
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLCorral : IABMeable<BE.Corral>
    {
        MapperCorral mapper = new MapperCorral();

        public int Alta(BE.Corral corral)
        {
            Validar(corral);
            try
            {
                return mapper.Alta(corral);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
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
                // 547 = el corral está siendo usado por otra tabla (FK).
                if (ex.Number == 547)
                    throw new ArgumentException("No se puede eliminar el corral porque tiene gallinas o producción cargada.");
                throw;
            }
        }

        public int Modificar(BE.Corral corral)
        {
            Validar(corral);
            try
            {
                return mapper.Modificar(corral);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
                throw;
            }
        }

        // 547 = el tipo o la ubicación elegidos no existen (FK).
        void TraducirError(SqlException ex)
        {
            if (ex.Number == 547)
                throw new ArgumentException("El tipo o la ubicación seleccionados no existen.");
        }

        public List<BE.Corral> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Corral> Buscar(BE.Corral filtro)
        {
            return mapper.Buscar(filtro);
        }

        public List<BE.TipoCorral> ListarTipos()
        {
            return mapper.ListarTipos();
        }

        public List<BE.Ubicacion> ListarUbicaciones()
        {
            return mapper.ListarUbicaciones();
        }

        void Validar(BE.Corral corral)
        {
            if (corral == null)
                throw new ArgumentException("No hay datos del corral.");

            if (corral.NroCorral == null)
                corral.NroCorral = "";
            corral.NroCorral = corral.NroCorral.Trim();
            if (corral.NroCorral == "")
                throw new ArgumentException("Ingrese el número de corral.");
            if (corral.NroCorral.Length > 10)
                throw new ArgumentException("El número de corral no puede tener más de 10 caracteres.");
            if (corral.IdTipoCorral <= 0)
                throw new ArgumentException("Seleccione el tipo de corral.");
            if (corral.Capacidad < 0)
                throw new ArgumentException("La capacidad no puede ser negativa.");
            // La ubicación es opcional: 0 (o nada elegido) se guarda como NULL.
            if (corral.IdUbicacion != null && corral.IdUbicacion <= 0)
                corral.IdUbicacion = null;

            // El número es único: se avisa antes de que lo rechace la base.
            foreach (BE.Corral otro in mapper.Listar())
            {
                if (otro.IdCorral != corral.IdCorral && otro.NroCorral.ToUpper() == corral.NroCorral.ToUpper())
                    throw new ArgumentException("Ya existe un corral con el número " + corral.NroCorral + ".");
            }
        }
    }
}
