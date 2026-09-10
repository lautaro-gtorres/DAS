using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapProfesion
    {
        Acceso acc = new Acceso();
        public int AltaProfesion(BE.Profesion entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            //parametro[0] = new SqlParameter("@IdAlumno", alum.IdAlumno);
            
            parametro[0] = new SqlParameter("@descProfesion", entidad.DescProfesion);

            fa = acc.Escribir("AltaTipoProfesion", parametro);
            return fa;
        }
        public int BajaProfesion(int id)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter("@IdProfesion", id);
            fa = acc.Escribir("BajaTipoProfesion", parametro);
            return fa;
        }

        public List<BE.Profesion> ListarProfesion()
        {
            List<BE.Profesion> profesiones = new List<BE.Profesion>();
            var dt = acc.Leer("ListadoTipoProfesion");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Profesion profesion = new BE.Profesion();
                profesion.IdProfesion = Convert.ToInt32(row["IdProfesion"]);
                profesion.DescProfesion = row["DescProfesion"].ToString();
               
                profesiones.Add(profesion);
            }
            return profesiones;
        }
        public int ModificarProfesion(BE.Profesion entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[2];
            parametro[0] = new SqlParameter("@IdProfesion", entidad.IdProfesion);
            parametro[1] = new SqlParameter("@descProfesion", entidad.DescProfesion);
            fa = acc.Escribir("UpdateTipoProfesion", parametro);
            return fa;
        }
        public List<BE.Profesion> BuscarProfesion(BE.Profesion entidad)
        {
            List<BE.Profesion> ls = new List<BE.Profesion>();

            List<SqlParameter> parametros = new List<SqlParameter>();

           
            if (entidad.IdProfesion > 0) parametros.Add(new SqlParameter("idProfesion", entidad.IdProfesion));
            if (!string.IsNullOrEmpty(entidad.DescProfesion) && entidad.DescProfesion.Length > 0)
                parametros.Add(new SqlParameter("descProfesion", entidad.DescProfesion));

            DataTable tabla = acc.Leer("BuscarTipoProfesion", parametros.ToArray());
            foreach (DataRow dr in tabla.Rows)
            {
                BE.Profesion ent = new BE.Profesion();
                ent.IdProfesion = int.Parse(dr["IdProfesion"].ToString());
                
                ent.DescProfesion = dr["DescProfesion"].ToString();
                ls.Add(ent);
            }
            return ls;



        }
    }
}

            
      

