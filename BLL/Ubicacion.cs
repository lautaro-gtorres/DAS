using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Ubicacion : IABMeable<BE.Ubicacion>
    {
        DAL.MapUbicacion mapper = new DAL.MapUbicacion();
        public int Alta(BE.Ubicacion entidad)
        {
            throw new NotImplementedException();
        }

        public int Baja(int id)
        {
            throw new NotImplementedException();
        }

        public List<BE.Ubicacion> Buscar(BE.Ubicacion filtro)
        {
            throw new NotImplementedException();
        }

        public List<BE.Ubicacion> Listar()
        {
            return mapper.ListarUbicaciones();
                
        }

        public int Modificar(BE.Ubicacion entidad)
        {
            throw new NotImplementedException();
        }
    }
}
