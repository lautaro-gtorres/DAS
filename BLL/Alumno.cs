using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Alumno
    {
        DAL.MapAlumno mapper = new DAL.MapAlumno();  
        public int AltaAlumno(BE.Alumno alumn)
        {
            int fa = 0;
            fa = mapper.AltaAlumno(alumn);
            return fa;
        }
        public List<BE.Alumno> ListarAlumnos()
        {
            List<BE.Alumno> alumnos = new List<BE.Alumno>();
            alumnos = mapper.ListarAlumnos();
            return alumnos;
        }
    }
}
