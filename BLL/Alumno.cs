using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Alumno : IABMeable<BE.Alumno>
    {
        DAL.MapAlumno mapper = new DAL.MapAlumno();
        public int Alta(BE.Alumno entidad)
        {
         return mapper.AltaAlumno(entidad);
        }

        public int Baja(int id)
        {
           return mapper.BajaAlumno(id);
        }
        public List<BE.Alumno> Listar()
        {

            return mapper.ListarAlumnos();
        }
        public int Modificar(BE.Alumno entidad)
        {
            return mapper.ModificarAlumno(entidad);
        }
        public List<BE.Alumno> Buscar(BE.Alumno filtro)
        {
            return mapper.Buscar(filtro);
        }
    }
}
