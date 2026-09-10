using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapPersona
    {
        Acceso acc = new Acceso();
        public int AltaPersona(BE.Persona entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[6];
            //parametro[0] = new SqlParameter("@IdAlumno", alum.IdAlumno);
            parametro[0] = new SqlParameter("@nombre", entidad.Nombre);
            parametro[1] = new SqlParameter("@apellido", entidad.Apellido);
            parametro[2] = new SqlParameter("@edad", entidad.Edad);
            parametro[3] = new SqlParameter("@sexo", entidad.Sexo);
            parametro[4] = new SqlParameter("@idNacionalidad", entidad.Idnacionalidad);
            parametro[5] = new SqlParameter("@idProfesion", entidad.IdProfesion);

            fa = acc.Escribir("AltaPersona", parametro);
            return fa;
        }
        public int BajaPersona(int id)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[1];
            parametro[0] = new SqlParameter("@IdPersona", id);
            fa = acc.Escribir("BajaPersona", parametro);
            return fa;
        }

        public List<BE.Persona> ListarPersona()
        {
            List<BE.Persona> personas = new List<BE.Persona>();
            var dt = acc.Leer("ListadoPersona");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Persona persona = new BE.Persona();
                persona.IdPersona = Convert.ToInt32(row["IdPersona"]);
                persona.Nombre = row["Nombre"].ToString();
                persona.Apellido = row["Apellido"].ToString();
                persona.Edad = Convert.ToInt32(row["Edad"]);
                persona.Sexo = row["Sexo"].ToString();
                persona.Idnacionalidad = Convert.ToInt32(row["IdNacionalidad"]);
                persona.IdProfesion = Convert.ToInt32(row["IdProfesion"]);
                personas.Add(persona);
            }
            return personas;
        }
        public int ModificarPersona(BE.Persona entidad)
        {
            int fa = 0;
            SqlParameter[] parametro = new SqlParameter[7];
            parametro[0] = new SqlParameter("@IdPersona", entidad.IdPersona);
            parametro[1] = new SqlParameter("@nombre", entidad.Nombre);
            parametro[2] = new SqlParameter("@apellido", entidad.Apellido);
            parametro[3] = new SqlParameter("@edad", entidad.Edad);
            parametro[4] = new SqlParameter("@sexo", entidad.Sexo);
            parametro[5] = new SqlParameter("@idNacionalidad", entidad.Idnacionalidad);
            parametro[6] = new SqlParameter("@idProfesion", entidad.IdProfesion);
            fa = acc.Escribir("UpdatePersona", parametro);
            return fa;
        }
        public List<BE.Persona> BuscarPersona(BE.Persona entidad)
        {
            List<BE.Persona> ls = new List<BE.Persona>();

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(entidad.Nombre) && entidad.Nombre.Length > 0)
                parametros.Add(new SqlParameter("nombre", entidad.Nombre));
            if (entidad.IdPersona > 0) parametros.Add(new SqlParameter("idPersona", entidad.IdPersona));
            if (entidad.Edad > 0) parametros.Add(new SqlParameter("edad", entidad.Edad));

            DataTable tabla = acc.Leer("BuscarPersona", parametros.ToArray());
            foreach (DataRow dr in tabla.Rows)
            {
                BE.Persona ent = new BE.Persona();
                ent.Nombre = dr["Nombre"].ToString();
                ent.Edad = int.Parse(dr["Edad"].ToString());
                ent.IdPersona = int.Parse(dr["IdPersona"].ToString());
                ls.Add(ent);
            }
            return ls;



        }
    }
}
