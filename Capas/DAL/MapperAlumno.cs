using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapperAlumno
    {
        Acceso acceso = new Acceso();

        // ALTA
        public int AltaAlumno(BE.Alumno alum)
        {
            int fa = 0;

            SqlParameter[] parametros = new SqlParameter[5];

            parametros[0] = new SqlParameter("@idAlumno", alum.IDAlumno);
            parametros[1] = new SqlParameter("@nombre", alum.Nombre);
            parametros[2] = new SqlParameter("@apellido", alum.Apellido);
            parametros[3] = new SqlParameter("@edad", alum.Edad);
            parametros[4] = new SqlParameter("@genero", alum.Genero);

            fa = acceso.Escribir("AltaAlumno", parametros);

            return fa;
        }

        // LISTAR
        public DataTable ListarAlumno()
        {
            return acceso.Leer("ListarAlumno", null);
        }

        // BAJA
        public int BajaAlumno(int idAlumno)
        {
            SqlParameter[] parametros = new SqlParameter[1];

            parametros[0] = new SqlParameter("@IdAlumno", idAlumno);

            return acceso.Escribir("BajaAlumno", parametros);
        }

        // EDITAR
        public int EditarAlumno(BE.Alumno alum)
        {
            SqlParameter[] parametros = new SqlParameter[5];

            parametros[0] = new SqlParameter("@idAlumno", alum.IDAlumno);
            parametros[1] = new SqlParameter("@nombre", alum.Nombre);
            parametros[2] = new SqlParameter("@apellido", alum.Apellido);
            parametros[3] = new SqlParameter("@edad", alum.Edad);
            parametros[4] = new SqlParameter("@genero", alum.Genero);

            return acceso.Escribir("EditarAlumno", parametros);
        }
    }
}
