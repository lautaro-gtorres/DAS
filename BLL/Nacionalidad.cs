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
        public int Alta(BE.Nacionalidad entidad)
        {
            return mapper.AltaNacionalidad(entidad);
            throw new NotImplementedException();
        }

        public int Baja(int id)
        {
            return mapper.BajaNacionalidad(id);
            throw new NotImplementedException();
        }

        public List<BE.Nacionalidad> Buscar(BE.Nacionalidad filtro)
        {
            return mapper.BuscarNacionalidad(filtro);
            throw new NotImplementedException();
        }

        public List<BE.Nacionalidad> Listar()
        {
            return mapper.ListarNacionalidad();
            throw new NotImplementedException();
        }

        public int Modificar(BE.Nacionalidad entidad)
        {
            return mapper.ModificarNacionalidad(entidad);
            throw new NotImplementedException();
        }
    }
}
