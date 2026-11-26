using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    public class MapperGallina : IMapper<BE.Gallina>
    {
        public int Alta(BE.Gallina gallina)
        {
            return Acceso.Instancia.Escribir("AltaGallina", Parametros(gallina, false));
        }

        // Baja lógica: la gallina queda con Activo = 0.
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idGallina", id);
            return Acceso.Instancia.Escribir("BajaGallina", parametros);
        }

        public int Modificar(BE.Gallina gallina)
        {
            return Acceso.Instancia.Escribir("ModificarGallina", Parametros(gallina, true));
        }

        // Solo gallinas activas.
        public List<BE.Gallina> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarGallinas"));
        }

        // Busca por código (contiene el texto de filtro.CodGallina).
        public List<BE.Gallina> Buscar(BE.Gallina filtro)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@codGallina", filtro.CodGallina ?? "");
            return Mapear(Acceso.Instancia.Leer("BuscarGallina", parametros));
        }

        // Catálogo para el ComboBox de tipo de gallina.
        public List<BE.TipoGallina> ListarTipos()
        {
            List<BE.TipoGallina> lista = new List<BE.TipoGallina>();
            foreach (DataRow row in Acceso.Instancia.Leer("ListarTiposGallina").Rows)
            {
                BE.TipoGallina tipo = new BE.TipoGallina();
                tipo.IdTipoGallina = Convert.ToInt32(row["IdTipoGallina"]);
                tipo.Descripcion = row["Descripcion"].ToString();
                lista.Add(tipo);
            }
            return lista;
        }

        // Alta y Modificar usan los mismos parámetros; Modificar agrega el Id.
        SqlParameter[] Parametros(BE.Gallina gallina, bool conId)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            if (conId)
                parametros.Add(new SqlParameter("@idGallina", gallina.IdGallina));
            parametros.Add(new SqlParameter("@codGallina", gallina.CodGallina));
            parametros.Add(new SqlParameter("@idRaza", gallina.IdRaza));
            parametros.Add(new SqlParameter("@idTipoGallina", gallina.IdTipoGallina));
            parametros.Add(new SqlParameter("@sexo", gallina.Sexo.ToString()));
            parametros.Add(new SqlParameter("@fechaNacimiento", SqlDbType.Date) { Value = (object)gallina.FechaNacimiento ?? DBNull.Value });
            parametros.Add(new SqlParameter("@idCorral", gallina.IdCorral));
            parametros.Add(new SqlParameter("@detalle", (object)gallina.Detalle ?? DBNull.Value));
            return parametros.ToArray();
        }

        List<BE.Gallina> Mapear(DataTable dt)
        {
            List<BE.Gallina> lista = new List<BE.Gallina>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Gallina gallina = new BE.Gallina();
                gallina.IdGallina = Convert.ToInt32(row["IdGallina"]);
                gallina.CodGallina = row["CodGallina"] == DBNull.Value ? null : row["CodGallina"].ToString();
                gallina.IdRaza = Convert.ToInt32(row["IdRaza"]);
                gallina.Raza = row["Raza"].ToString();
                gallina.IdTipoGallina = Convert.ToInt32(row["IdTipoGallina"]);
                gallina.TipoGallina = row["TipoGallina"].ToString();
                gallina.Sexo = Convert.ToChar(row["Sexo"]);
                gallina.FechaNacimiento = row["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaNacimiento"]);
                gallina.IdCorral = Convert.ToInt32(row["IdCorral"]);
                gallina.NroCorral = row["NroCorral"].ToString();
                gallina.Detalle = row["Detalle"] == DBNull.Value ? null : row["Detalle"].ToString();
                gallina.Activo = Convert.ToBoolean(row["Activo"]);
                lista.Add(gallina);
            }
            return lista;
        }
    }
}
