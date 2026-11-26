using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Reglas de negocio del ABM de corrales.
    /// Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    /// </summary>
    public class BLLCorral : IABMeable<BE.Corral>
    {
        MapperCorral mapper = new MapperCorral();

        public int Alta(BE.Corral corral)
        {
            Validar(corral);
            return mapper.Alta(corral);
        }

        public int Baja(int id)
        {
            return ErrorBD.Ejecutar(() => mapper.Baja(id),
                mensajeEnUso: "No se puede eliminar el corral porque tiene gallinas o producción cargada.");
        }

        public int Modificar(BE.Corral corral)
        {
            Validar(corral);
            return mapper.Modificar(corral);
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

        void Validar(BE.Corral corral)
        {
            if (corral == null)
                throw new ArgumentException("No hay datos del corral.");

            corral.NroCorral = (corral.NroCorral ?? "").Trim();
            if (corral.NroCorral == "")
                throw new ArgumentException("Ingrese el número de corral.");
            if (corral.NroCorral.Length > 10)
                throw new ArgumentException("El número de corral no puede tener más de 10 caracteres.");
            if (corral.IdTipoCorral <= 0)
                throw new ArgumentException("Seleccione el tipo de corral.");
            if (corral.Capacidad < 0)
                throw new ArgumentException("La capacidad no puede ser negativa.");

            // El número es único: se avisa antes de que lo rechace la base.
            bool repetido = mapper.Listar().Any(c =>
                c.IdCorral != corral.IdCorral &&
                string.Equals(c.NroCorral, corral.NroCorral, StringComparison.OrdinalIgnoreCase));
            if (repetido)
                throw new ArgumentException("Ya existe un corral con el número " + corral.NroCorral + ".");
        }
    }
}
