using BE;
using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
  
    public class BLLUsuario : IABMeable<BE.Usuario>
    {
        MapperUsuario map = new MapperUsuario();
        public int Alta(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public int Baja(int id)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> Buscar(Usuario filtro)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> Listar()
        {
            throw new NotImplementedException();
        }

        public int Modificar(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public bool ValidarUsuario(string usuario, string password)
        {
            
            return map.ValidarUsuario(usuario, password);
        }
    }
}
