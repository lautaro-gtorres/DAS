using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Nacionalidad : IABMeable<BE.Nacionalidad>
    {
        DAL.MapNacionalidad mapper = new DAL.MapNacionalidad();

        void Validar(BE.Nacionalidad entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.DescNacionalidad))
                throw new ArgumentException("La nacionalidad no puede estar vacía.");
        }

        public int Alta(BE.Nacionalidad entidad)
        {
            Validar(entidad);
            return mapper.AltaNacionalidad(entidad);
        }

        public int Baja(int id)
        {
            return mapper.BajaNacionalidad(id);
        }

        public List<BE.Nacionalidad> Buscar(BE.Nacionalidad filtro)
        {
            return mapper.BuscarNacionalidad(filtro);
        }

        public List<BE.Nacionalidad> Listar()
        {
            return mapper.ListarNacionalidad();
        }

        public int Modificar(BE.Nacionalidad entidad)
        {
            Validar(entidad);
            return mapper.ModificarNacionalidad(entidad);
        }
    }
}
