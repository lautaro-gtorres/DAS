using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Alumno
    {
        DAL.MapperAlumno mapper = new DAL.MapperAlumno();

        // ALTA
        public int AltaAlumno(BE.Alumno alumn)
        {
            int fa = 0;

            fa = mapper.AltaAlumno(alumn);

            return fa;
        }

        // LISTAR
        public DataTable ListarAlumno()
        {
            return mapper.ListarAlumno();
        }

        // BAJA
        public int BajaAlumno(int idAlumno)
        {
            return mapper.BajaAlumno(idAlumno);
        }

        // EDITAR
        public int EditarAlumno(BE.Alumno alumn)
        {
            return mapper.EditarAlumno(alumn);
        }
    }
}
