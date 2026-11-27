using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de gallinas (baja lógica).
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLGallina : IABMeable<BE.Gallina>
    {
        MapperGallina mapper = new MapperGallina();
        MapperCorral mapperCorral = new MapperCorral();

        public int Alta(BE.Gallina gallina)
        {
            Validar(gallina);
            try
            {
                return mapper.Alta(gallina);
            }
            catch (SqlException ex)
            {
                // 2627 = dato repetido. El código es único también entre las gallinas dadas de baja.
                if (ex.Number == 2627)
                    throw new ArgumentException("Ya existe una gallina con el código " + gallina.CodGallina + " (puede estar dada de baja).");
                throw;
            }
        }

        // Baja lógica: deja de aparecer en los listados pero queda en la base.
        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Gallina gallina)
        {
            Validar(gallina);
            try
            {
                return mapper.Modificar(gallina);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    throw new ArgumentException("Ya existe una gallina con el código " + gallina.CodGallina + " (puede estar dada de baja).");
                throw;
            }
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

        void Validar(BE.Gallina gallina)
        {
            if (gallina == null)
                throw new ArgumentException("No hay datos de la gallina.");

            if (gallina.CodGallina == null)
                gallina.CodGallina = "";
            gallina.CodGallina = gallina.CodGallina.Trim();
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

            if (gallina.FechaNacimiento != null && gallina.FechaNacimiento.Value.Date > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura.");

            // El detalle es opcional: si viene vacío se guarda como NULL.
            if (string.IsNullOrWhiteSpace(gallina.Detalle))
                gallina.Detalle = null;
            else
                gallina.Detalle = gallina.Detalle.Trim();
            if (gallina.Detalle != null && gallina.Detalle.Length > 500)
                throw new ArgumentException("El detalle no puede tener más de 500 caracteres.");

            ValidarCorral(gallina);
        }

        // El corral tiene que existir y no puede superar su capacidad (si la tiene cargada).
        void ValidarCorral(BE.Gallina gallina)
        {
            if (gallina.IdCorral <= 0)
                throw new ArgumentException("Seleccione el corral.");

            BE.Corral corral = null;
            foreach (BE.Corral c in mapperCorral.Listar())
            {
                if (c.IdCorral == gallina.IdCorral)
                    corral = c;
            }
            if (corral == null)
                throw new ArgumentException("El corral seleccionado no existe.");
            if (corral.Capacidad == null)
                return;

            // Se cuentan las otras gallinas activas del corral (sin contar la que se está modificando).
            int ocupadas = 0;
            foreach (BE.Gallina g in mapper.Listar())
            {
                if (g.IdCorral == corral.IdCorral && g.IdGallina != gallina.IdGallina)
                    ocupadas++;
            }
            if (ocupadas >= corral.Capacidad.Value)
                throw new ArgumentException("El corral " + corral.NroCorral + " está completo (capacidad " + corral.Capacidad + ").");
        }
    }
}
