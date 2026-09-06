using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IABMeable<T>
    {
        int Alta(T entidad);
        int Baja(int id);
        int Modificar(T entidad);
        List<T> Listar();
        List<T> Buscar(T filtro);
    }
}
