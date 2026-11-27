using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Arma el árbol de la granja (patrón Composite) con los corrales, sus gallinas activas
    // y la producción del último mes:  Granja → Ubicación → Corral.
    public class BLLGranja
    {
        public const string SinUbicacion = "Sin ubicación";

        MapperCorral mapperCorral = new MapperCorral();
        MapperGallina mapperGallina = new MapperGallina();
        MapperProduccion mapperProduccion = new MapperProduccion();

        public BE.ComponenteGranja ArmarEstructura()
        {
            BE.GrupoGranja granja = new BE.GrupoGranja("Granja LRChickens", "Granja");
            List<BE.Gallina> gallinas = mapperGallina.Listar();
            List<BE.Produccion> produccion = mapperProduccion.Buscar(null, DateTime.Today.AddMonths(-1), DateTime.Today);

            foreach (BE.Corral corral in mapperCorral.Listar())
            {
                BE.CorralGranja hoja = new BE.CorralGranja(corral.NroCorral, ContarGallinas(gallinas, corral.IdCorral),
                    CapacidadDe(corral), SumarHuevos(produccion, corral.IdCorral));

                string ubicacion = corral.Ubicacion;
                if (string.IsNullOrEmpty(ubicacion))
                    ubicacion = SinUbicacion;
                BuscarOCrearUbicacion(granja, ubicacion).Agregar(hoja);
            }
            return granja;
        }

        // Busca el grupo de la ubicación entre los hijos de la granja; si no está, lo crea.
        BE.GrupoGranja BuscarOCrearUbicacion(BE.GrupoGranja granja, string nombre)
        {
            foreach (BE.ComponenteGranja hijo in granja.Hijos())
            {
                if (hijo.Nombre == nombre)
                    return (BE.GrupoGranja)hijo;
            }
            BE.GrupoGranja nuevo = new BE.GrupoGranja(nombre, "Ubicación");
            granja.Agregar(nuevo);
            return nuevo;
        }

        int ContarGallinas(List<BE.Gallina> gallinas, int idCorral)
        {
            int cantidad = 0;
            foreach (BE.Gallina gallina in gallinas)
            {
                if (gallina.IdCorral == idCorral)
                    cantidad++;
            }
            return cantidad;
        }

        int SumarHuevos(List<BE.Produccion> produccion, int idCorral)
        {
            int huevos = 0;
            foreach (BE.Produccion p in produccion)
            {
                if (p.IdCorral == idCorral)
                    huevos = huevos + p.CantidadHuevos;
            }
            return huevos;
        }

        int CapacidadDe(BE.Corral corral)
        {
            if (corral.Capacidad == null)
                return 0;
            return corral.Capacidad.Value;
        }
    }
}
