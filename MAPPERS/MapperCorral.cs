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
    public class MapperCorral : IMapper<BE.Corral>
    {
        public int Alta(BE.Corral corral)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@nroCorral", corral.NroCorral);
            parametros[1] = new SqlParameter("@idTipoCorral", corral.IdTipoCorral);
            parametros[2] = new SqlParameter("@capacidad", (object)corral.Capacidad ?? DBNull.Value);
            return Acceso.Instancia.Escribir("AltaCorral", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idCorral", id);
            return Acceso.Instancia.Escribir("BajaCorral", parametros);
        }

        public int Modificar(BE.Corral corral)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idCorral", corral.IdCorral);
            parametros[1] = new SqlParameter("@nroCorral", corral.NroCorral);
            parametros[2] = new SqlParameter("@idTipoCorral", corral.IdTipoCorral);
            parametros[3] = new SqlParameter("@capacidad", (object)corral.Capacidad ?? DBNull.Value);
            return Acceso.Instancia.Escribir("ModificarCorral", parametros);
        }

        public List<BE.Corral> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarCorrales"));
        }

        // Busca por número de corral (contiene el texto de filtro.NroCorral).
        public List<BE.Corral> Buscar(BE.Corral filtro)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nroCorral", filtro.NroCorral ?? "");
            return Mapear(Acceso.Instancia.Leer("BuscarCorral", parametros));
        }

        // Catálogo para el ComboBox de tipo de corral.
        public List<BE.TipoCorral> ListarTipos()
        {
            List<BE.TipoCorral> lista = new List<BE.TipoCorral>();
            foreach (DataRow row in Acceso.Instancia.Leer("ListarTiposCorral").Rows)
            {
                BE.TipoCorral tipo = new BE.TipoCorral();
                tipo.IdTipoCorral = Convert.ToInt32(row["IdTipoCorral"]);
                tipo.Descripcion = row["Descripcion"].ToString();
                lista.Add(tipo);
            }
            return lista;
        }

        // ---- Filas de la base -> entidades BE ----

        List<BE.Corral> Mapear(DataTable dt)
        {
            List<BE.Corral> lista = new List<BE.Corral>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Corral corral = new BE.Corral();
                corral.IdCorral = Convert.ToInt32(row["IdCorral"]);
                corral.NroCorral = row["NroCorral"].ToString();
                corral.IdTipoCorral = Convert.ToInt32(row["IdTipoCorral"]);
                corral.TipoCorral = row["TipoCorral"].ToString();
                corral.Capacidad = row["Capacidad"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Capacidad"]);
                lista.Add(corral);
            }
            return lista;
        }
    }
}
