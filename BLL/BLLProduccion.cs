using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Reglas de negocio de la producción diaria de huevos por corral.
    /// Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    /// </summary>
    public class BLLProduccion : IABMeable<BE.Produccion>
    {
        MapperProduccion mapper = new MapperProduccion();

        public int Alta(BE.Produccion produccion)
        {
            Validar(produccion);
            return ErrorBD.Ejecutar(() => mapper.Alta(produccion), MensajeRepetido(produccion), "El corral seleccionado no existe.");
        }

        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Produccion produccion)
        {
            Validar(produccion);
            return ErrorBD.Ejecutar(() => mapper.Modificar(produccion), MensajeRepetido(produccion), "El corral seleccionado no existe.");
        }

        public List<BE.Produccion> Listar()
        {
            return mapper.Listar();
        }

        // Filtra por corral (IdCorral > 0) y/o por día (Fecha cargada).
        public List<BE.Produccion> Buscar(BE.Produccion filtro)
        {
            return mapper.Buscar(filtro);
        }

        // Filtros opcionales: los que vienen en null no filtran.
        public List<BE.Produccion> Buscar(int? idCorral, DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");
            return mapper.Buscar(idCorral, desde, hasta);
        }

        static string MensajeRepetido(BE.Produccion produccion)
        {
            return "Ya hay producción cargada para ese corral el " + produccion.Fecha.ToString("dd/MM/yyyy") + ".";
        }

        void Validar(BE.Produccion produccion)
        {
            if (produccion == null)
                throw new ArgumentException("No hay datos de la producción.");
            if (produccion.IdCorral <= 0)
                throw new ArgumentException("Seleccione el corral.");
            if (produccion.Fecha == default(DateTime))
                throw new ArgumentException("Ingrese la fecha.");

            produccion.Fecha = produccion.Fecha.Date;
            if (produccion.Fecha > DateTime.Today)
                throw new ArgumentException("La fecha no puede ser futura.");
            if (produccion.CantidadHuevos < 0)
                throw new ArgumentException("La cantidad de huevos no puede ser negativa.");
        }
    }
}
