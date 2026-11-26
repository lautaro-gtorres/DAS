using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Reglas de negocio del ABM de gallinas (baja lógica).
    /// Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    /// </summary>
    public class BLLGallina : IABMeable<BE.Gallina>
    {
        MapperGallina mapper = new MapperGallina();
        MapperCorral mapperCorral = new MapperCorral();

        public int Alta(BE.Gallina gallina)
        {
            Validar(gallina);
            return ErrorBD.Ejecutar(() => mapper.Alta(gallina), MensajeRepetido(gallina));
        }

        // Baja lógica: deja de aparecer en los listados pero queda en la base.
        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Gallina gallina)
        {
            Validar(gallina);
            return ErrorBD.Ejecutar(() => mapper.Modificar(gallina), MensajeRepetido(gallina));
        }

        public List<BE.Gallina> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Gallina> Buscar(BE.Gallina filtro)
        {
            return mapper.Buscar(filtro);
        }

        public List<BE.TipoGallina> ListarTipos()
        {
            return mapper.ListarTipos();
        }

        static string MensajeRepetido(BE.Gallina gallina)
        {
            // El código es único también entre las gallinas dadas de baja.
            return "Ya existe una gallina con el código " + gallina.CodGallina + " (puede estar dada de baja).";
        }

        void Validar(BE.Gallina gallina)
        {
            if (gallina == null)
                throw new ArgumentException("No hay datos de la gallina.");

            gallina.CodGallina = (gallina.CodGallina ?? "").Trim();
            if (gallina.CodGallina == "")
                throw new ArgumentException("Ingrese el código de la gallina.");
            if (gallina.CodGallina.Length > 20)
                throw new ArgumentException("El código no puede tener más de 20 caracteres.");
            if (gallina.IdRaza <= 0)
                throw new ArgumentException("Seleccione la raza.");
            if (gallina.IdTipoGallina <= 0)
                throw new ArgumentException("Seleccione el tipo de gallina.");

            gallina.Sexo = char.ToUpper(gallina.Sexo);
            if (gallina.Sexo != 'M' && gallina.Sexo != 'H')
                throw new ArgumentException("Seleccione el sexo (M = macho, H = hembra).");

            if (gallina.FechaNacimiento.HasValue && gallina.FechaNacimiento.Value.Date > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura.");

            gallina.Detalle = string.IsNullOrWhiteSpace(gallina.Detalle) ? null : gallina.Detalle.Trim();
            if (gallina.Detalle != null && gallina.Detalle.Length > 500)
                throw new ArgumentException("El detalle no puede tener más de 500 caracteres.");

            ValidarCorral(gallina);
        }

        // El corral tiene que existir y no puede superar su capacidad (si la tiene cargada).
        void ValidarCorral(BE.Gallina gallina)
        {
            if (gallina.IdCorral <= 0)
                throw new ArgumentException("Seleccione el corral.");

            BE.Corral corral = mapperCorral.Listar().FirstOrDefault(c => c.IdCorral == gallina.IdCorral);
            if (corral == null)
                throw new ArgumentException("El corral seleccionado no existe.");
            if (!corral.Capacidad.HasValue)
                return;

            int ocupadas = mapper.Listar().Count(g => g.IdCorral == corral.IdCorral && g.IdGallina != gallina.IdGallina);
            if (ocupadas >= corral.Capacidad.Value)
                throw new ArgumentException("El corral " + corral.NroCorral + " está completo (capacidad " + corral.Capacidad + ").");
        }
    }
}
