using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLVehiculo
    {
        MAPPERS.MapperVehiculo mapper = new MAPPERS.MapperVehiculo();

        public List<BE.Vehiculo> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Vehiculo> ListarDisponibles()
        {
            return mapper.Listar().Where(v => v.Disponible).ToList();
        }

        public int Liberar(int idVehiculo)
        {
            return mapper.Liberar(idVehiculo);
        }
    }
}
