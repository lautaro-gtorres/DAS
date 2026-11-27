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
    public class MapperProduccion : IMapper<BE.Produccion>
    {
        public int Alta(BE.Produccion produccion)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idCorral", produccion.IdCorral);
            parametros[1] = new SqlParameter("@fecha", SqlDbType.Date);
            parametros[1].Value = produccion.Fecha;
            parametros[2] = new SqlParameter("@cantidadHuevos", produccion.CantidadHuevos);
            return Acceso.Instancia.Escribir("AltaProduccion", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idProduccion", id);
            return Acceso.Instancia.Escribir("BajaProduccion", parametros);
        }

        public int Modificar(BE.Produccion produccion)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idProduccion", produccion.IdProduccion);
            parametros[1] = new SqlParameter("@idCorral", produccion.IdCorral);
            parametros[2] = new SqlParameter("@fecha", SqlDbType.Date);
            parametros[2].Value = produccion.Fecha;
            parametros[3] = new SqlParameter("@cantidadHuevos", produccion.CantidadHuevos);
            return Acceso.Instancia.Escribir("ModificarProduccion", parametros);
        }

        public List<BE.Produccion> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarProducciones"));
        }

        // Filtra por el corral (si IdCorral > 0) y por el día (si Fecha tiene valor).
        public List<BE.Produccion> Buscar(BE.Produccion filtro)
        {
            int? idCorral = null;
            if (filtro.IdCorral > 0)
                idCorral = filtro.IdCorral;

            DateTime? dia = null;
            if (filtro.Fecha != DateTime.MinValue)
                dia = filtro.Fecha.Date;

            return Buscar(idCorral, dia, dia);
        }

        // Filtros opcionales: los que vienen en null no filtran (el SP los ignora).
        public List<BE.Produccion> Buscar(int? idCorral, DateTime? desde, DateTime? hasta)
        {
            SqlParameter[] parametros = new SqlParameter[3];

            parametros[0] = new SqlParameter("@idCorral", SqlDbType.Int);
            if (idCorral == null)
                parametros[0].Value = DBNull.Value;
            else
                parametros[0].Value = idCorral.Value;

            parametros[1] = new SqlParameter("@fechaDesde", SqlDbType.Date);
            if (desde == null)
                parametros[1].Value = DBNull.Value;
            else
                parametros[1].Value = desde.Value;

            parametros[2] = new SqlParameter("@fechaHasta", SqlDbType.Date);
            if (hasta == null)
                parametros[2].Value = DBNull.Value;
            else
                parametros[2].Value = hasta.Value;

            return Mapear(Acceso.Instancia.Leer("BuscarProduccion", parametros));
        }

        List<BE.Produccion> Mapear(DataTable dt)
        {
            List<BE.Produccion> lista = new List<BE.Produccion>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Produccion produccion = new BE.Produccion();
                produccion.IdProduccion = Convert.ToInt32(row["IdProduccion"]);
                produccion.IdCorral = Convert.ToInt32(row["IdCorral"]);
                produccion.NroCorral = row["NroCorral"].ToString();
                produccion.Fecha = Convert.ToDateTime(row["Fecha"]);
                produccion.CantidadHuevos = Convert.ToInt32(row["CantidadHuevos"]);
                lista.Add(produccion);
            }
            return lista;
        }
    }
}
