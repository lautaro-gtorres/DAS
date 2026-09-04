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
         return mapper.AltaAlumno(alumn);
        }

        public int BajaAlumno(int idAlumno)
        {
           return mapper.BajaAlumno(idAlumno);
        }
        public List<BE.Alumno> ListarAlumnos()
        {
            
            return mapper.ListarAlumnos();
        }
        public int ModificarAlumno(BE.Alumno alumn)
        {
            return mapper.ModificarAlumno(alumn);
        }
    }
}
