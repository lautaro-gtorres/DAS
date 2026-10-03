using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Movimiento
    {
        Mapper.MapperMovimiento mapper = new Mapper.MapperMovimiento();

        // Estado de cuenta: solo los movimientos del usuario indicado (punto 4).
        public List<BE.Movimiento> ListarPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Usuario inválido.");
            return mapper.ListarPorUsuario(idUsuario);
        }
    }
}
