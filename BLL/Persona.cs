using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Persona : IABMeable<BE.Persona>
    {
        DAL.MapPersona mapper = new DAL.MapPersona();
        public int Alta(BE.Persona entidad)
        {
         return mapper.AltaPersona(entidad);
        }

        public int Baja(int id)
        {
           return mapper.BajaPersona(id);
        }
        public List<BE.Persona> Listar()
        {

            return mapper.ListarPersona();
        }
        public int Modificar(BE.Persona entidad)
        {
            return mapper.ModificarPersona(entidad);
        }
        public List<BE.Persona> Buscar(BE.Persona filtro)
        {
            return mapper.BuscarPersona(filtro);
        }
    }
}
