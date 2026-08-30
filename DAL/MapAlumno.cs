using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapAlumno
    {
        Acceso acc = new Acceso();
        public int AltaAlumno(BE.Alumno alum)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[4];
            //parametro[0] = new SqlParameter("@IdAlumno", alum.IdAlumno);
            parametro[0] = new SqlParameter("@nombre", alum.Nombre);
            parametro[1] = new SqlParameter("@apellido", alum.Apellido);
            parametro[2] = new SqlParameter("@edad", alum.Edad);
            parametro[3] = new SqlParameter("@genero", alum.Genero);

            fa = acc.Escribir("AltaAlumno", parametro);
            return fa;
        }

        public List<BE.Alumno> ListarAlumnos()
        {
            List<BE.Alumno> alumnos = new List<BE.Alumno>();
            var dt = acc.Leer();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Alumno alumno = new BE.Alumno();
                alumno.IdAlumno = Convert.ToInt32(row["IdAlumno"]);
                alumno.Nombre = row["Nombre"].ToString();
                alumno.Apellido = row["Apellido"].ToString();
                alumno.Edad = Convert.ToInt32(row["Edad"]);
                alumno.Genero = row["Genero"].ToString();
                alumnos.Add(alumno);
            }
            return alumnos;
        }
    }
}
