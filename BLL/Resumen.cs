using System.Collections.Generic;

namespace BLL
{
    public class Resumen
    {
        DAL.MapResumen mapper = new DAL.MapResumen();

        public BE.ResumenGeneral ObtenerResumenGeneral()
        {
            return mapper.ObtenerResumenGeneral();
        }

        public List<BE.ResumenNacionalidad> ObtenerResumenPorNacionalidad()
        {
            return mapper.ObtenerResumenPorNacionalidad();
        }

        public List<BE.ResumenProfesion> ObtenerResumenPorProfesion()
        {
            return mapper.ObtenerResumenPorProfesion();
        }
    }
}
