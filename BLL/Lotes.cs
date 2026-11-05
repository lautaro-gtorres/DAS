using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Lotes : IABMeable<BE.Lotes>
    {
        DAL.MapLotes mapper = new DAL.MapLotes();
        public int Alta(BE.Lotes entidad)
        {
         return mapper.AltaLote(entidad);
        }

        public int Baja(int id)
        {
           return mapper.BajaLote(id);
        }
        public List<BE.Lotes> Listar()
        {

            return mapper.ListarLotes();
        }
        public int Modificar(BE.Lotes entidad)
        {
            return mapper.ModificarLote(entidad);
        }
        public List<BE.Lotes> Buscar(BE.Lotes filtro)
        {
            return mapper.Buscar(filtro);
        }
    }
}
