using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapNacionalidad
    {
        Acceso acc = new Acceso();
        public int AltaNacionalidad(BE.Nacionalidad entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            //parametro[0] = new SqlParameter("@IdAlumno", alum.IdAlumno);
            
            parametro[0] = new SqlParameter("@descNacionalidad", entidad.DescNacionalidad);

            fa = acc.Escribir("AltaNacionalidad", parametro);
            return fa;
        }
        public int BajaNacionalidad(int id)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter("@idNacionalidad", id);
            fa = acc.Escribir("BajaNacionalidad", parametro);
            return fa;
        }         
     

        public List<BE.Nacionalidad> ListarNacionalidad()
        {
            List<BE.Nacionalidad> nacionalidades = new List<BE.Nacionalidad>();
            var dt = acc.Leer("ListadoNacionalidad");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Nacionalidad nacionalidad = new BE.Nacionalidad();
                nacionalidad.IdNacionalidad = Convert.ToInt32(row["IdNacionalidad"]);
                nacionalidad.DescNacionalidad = row["DescNacionalidad"].ToString();
                nacionalidades.Add(nacionalidad);
            }
            return nacionalidades;
        }
        public int ModificarNacionalidad(BE.Nacionalidad entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[2];
            parametro[0] = new SqlParameter("@idNacionalidad", entidad.IdNacionalidad);
            parametro[1] = new SqlParameter("@descNacionalidad", entidad.DescNacionalidad);
            fa = acc.Escribir("UpdateNacionalidad", parametro);
            return fa;
        }
        public List<BE.Nacionalidad> BuscarNacionalidad(BE.Nacionalidad entidad)
        {
            List<BE.Nacionalidad> ls = new List<BE.Nacionalidad>();

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(entidad.DescNacionalidad) && entidad.DescNacionalidad.Length > 0)
                parametros.Add(new SqlParameter("descNacionalidad", entidad.DescNacionalidad));
            if (entidad.IdNacionalidad > 0) parametros.Add(new SqlParameter("idNacionalidad", entidad.IdNacionalidad));

            DataTable tabla = acc.Leer("BuscarNacionalidad", parametros.ToArray());
            foreach (DataRow dr in tabla.Rows)
            {
                BE.Nacionalidad ent = new BE.Nacionalidad();
                ent.IdNacionalidad = int.Parse(dr["IdNacionalidad"].ToString());
                ent.DescNacionalidad = dr["DescNacionalidad"].ToString();
                ls.Add(ent);
            }
            return ls       ;
        }
       
    }
}
