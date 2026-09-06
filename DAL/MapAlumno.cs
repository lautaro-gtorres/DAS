using System;
using System.Collections.Generic;
using System.Data;
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
        public int BajaAlumno(int idAlumno)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter("@IdAlumno", idAlumno);
            fa = acc.Escribir("BajaAlumno", parametro);
            return fa;
        }

        public List<BE.Alumno> ListarAlumnos()
        {
            List<BE.Alumno> alumnos = new List<BE.Alumno>();
            var dt = acc.Leer("ListadoAlumno");
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
        public int ModificarAlumno(BE.Alumno alum)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[5];
            parametro[0] = new SqlParameter("@IdAlumno", alum.IdAlumno);
            parametro[1] = new SqlParameter("@nombre", alum.Nombre);
            parametro[2] = new SqlParameter("@apellido", alum.Apellido);
            parametro[3] = new SqlParameter("@edad", alum.Edad);
            parametro[4] = new SqlParameter("@genero", alum.Genero);
            fa = acc.Escribir("UpdateAlumno", parametro);
            return fa;
        }
        public List<BE.Alumno> Buscar(BE.Alumno alumno)
        {
            List<BE.Alumno> ls = new List<BE.Alumno>();

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (alumno.Nombre.Length > 0) parametros.Add(new SqlParameter("nombre", alumno.Nombre));
            if (alumno.IdAlumno > 0) parametros.Add(new SqlParameter("idAlumno", alumno.IdAlumno));
            if (alumno.Edad > 0) parametros.Add(new SqlParameter("edad", alumno.Edad));

            DataTable tabla = acc.Leer("BuscarAlumnos", parametros.ToArray());
            foreach (DataRow dr in tabla.Rows)
            {
                BE.Alumno alum = new BE.Alumno();
                alum.Nombre = dr["Nombre"].ToString();
                alum.Edad = int.Parse(dr["Edad"].ToString());
                alum.IdAlumno = int.Parse(dr["idAlumno"].ToString());
                ls.Add(alum);
            }
            return ls;



        }
    }
}
