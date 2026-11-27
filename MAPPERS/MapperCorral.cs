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
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@nroCorral", corral.NroCorral);
            parametros[1] = new SqlParameter("@idTipoCorral", corral.IdTipoCorral);
            parametros[2] = ParametroCapacidad(corral);
            parametros[3] = ParametroUbicacion(corral);
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
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@idCorral", corral.IdCorral);
            parametros[1] = new SqlParameter("@nroCorral", corral.NroCorral);
            parametros[2] = new SqlParameter("@idTipoCorral", corral.IdTipoCorral);
            parametros[3] = ParametroCapacidad(corral);
            parametros[4] = ParametroUbicacion(corral);
            return Acceso.Instancia.Escribir("ModificarCorral", parametros);
        }

        public List<BE.Corral> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarCorrales"));
        }

        // Busca por número de corral (contiene el texto de filtro.NroCorral).
        public List<BE.Corral> Buscar(BE.Corral filtro)
        {
            string nro = filtro.NroCorral;
            if (nro == null)
                nro = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nroCorral", nro);
            return Mapear(Acceso.Instancia.Leer("BuscarCorral", parametros));
        }

        // Catálogo para el ComboBox de tipo de corral.
        public List<BE.TipoCorral> ListarTipos()
        {
            List<BE.TipoCorral> lista = new List<BE.TipoCorral>();
            DataTable dt = Acceso.Instancia.Leer("ListarTiposCorral");
            foreach (DataRow row in dt.Rows)
            {
                BE.TipoCorral tipo = new BE.TipoCorral();
                tipo.IdTipoCorral = Convert.ToInt32(row["IdTipoCorral"]);
                tipo.Descripcion = row["Descripcion"].ToString();
                lista.Add(tipo);
            }
            return lista;
        }

        // Catálogo para el ComboBox de ubicación.
        public List<BE.Ubicacion> ListarUbicaciones()
        {
            List<BE.Ubicacion> lista = new List<BE.Ubicacion>();
            DataTable dt = Acceso.Instancia.Leer("ListarUbicaciones");
            foreach (DataRow row in dt.Rows)
            {
                BE.Ubicacion ubicacion = new BE.Ubicacion();
                ubicacion.IdUbicacion = Convert.ToInt32(row["IdUbicacion"]);
                ubicacion.Nombre = row["Nombre"].ToString();
                lista.Add(ubicacion);
            }
            return lista;
        }

        // La ubicación es opcional: si no tiene valor se manda NULL a la base.
        SqlParameter ParametroUbicacion(BE.Corral corral)
        {
            if (corral.IdUbicacion == null)
                return new SqlParameter("@idUbicacion", DBNull.Value);
            return new SqlParameter("@idUbicacion", corral.IdUbicacion.Value);
        }

        // La capacidad es opcional: si no tiene valor se manda NULL a la base.
        SqlParameter ParametroCapacidad(BE.Corral corral)
        {
            if (corral.Capacidad == null)
                return new SqlParameter("@capacidad", DBNull.Value);
            return new SqlParameter("@capacidad", corral.Capacidad.Value);
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
                if (row["Capacidad"] == DBNull.Value)
                    corral.Capacidad = null;
                else
                    corral.Capacidad = Convert.ToInt32(row["Capacidad"]);
                if (row["IdUbicacion"] == DBNull.Value)
                {
                    corral.IdUbicacion = null;
                }
                else
                {
                    corral.IdUbicacion = Convert.ToInt32(row["IdUbicacion"]);
                    corral.Ubicacion = row["Ubicacion"].ToString();
                }
                lista.Add(corral);
            }
            return lista;
        }
    }
}
