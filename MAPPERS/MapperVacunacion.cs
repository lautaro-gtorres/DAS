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
    public class MapperVacunacion : IMapper<BE.Vacunacion>
    {
        public int Alta(BE.Vacunacion vacunacion)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@nombre", vacunacion.Nombre);
            parametros[1] = new SqlParameter("@cantidad", vacunacion.Cantidad);
            parametros[2] = new SqlParameter("@idCorral", vacunacion.IdCorral);
            parametros[3] = new SqlParameter("@fechaAplicacion", vacunacion.FechaAplicacion.Date);
            parametros[4] = ParametroProxima(vacunacion);
            return Acceso.Instancia.Escribir("AltaVacunacion", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idVacunacion", id);
            return Acceso.Instancia.Escribir("BajaVacunacion", parametros);
        }

        public int Modificar(BE.Vacunacion vacunacion)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("@idVacunacion", vacunacion.IdVacunacion);
            parametros[1] = new SqlParameter("@nombre", vacunacion.Nombre);
            parametros[2] = new SqlParameter("@cantidad", vacunacion.Cantidad);
            parametros[3] = new SqlParameter("@idCorral", vacunacion.IdCorral);
            parametros[4] = new SqlParameter("@fechaAplicacion", vacunacion.FechaAplicacion.Date);
            parametros[5] = ParametroProxima(vacunacion);
            return Acceso.Instancia.Escribir("ModificarVacunacion", parametros);
        }

        public List<BE.Vacunacion> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarVacunaciones"));
        }

        // Busca por nombre de la vacuna (contiene el texto de filtro.Nombre).
        public List<BE.Vacunacion> Buscar(BE.Vacunacion filtro)
        {
            string nombre = filtro.Nombre;
            if (nombre == null)
                nombre = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombre", nombre);
            return Mapear(Acceso.Instancia.Leer("BuscarVacunacion", parametros));
        }

        // La próxima aplicación es opcional: si no tiene valor se manda NULL a la base.
        SqlParameter ParametroProxima(BE.Vacunacion vacunacion)
        {
            if (vacunacion.ProximaAplicacion == null)
                return new SqlParameter("@proximaAplicacion", DBNull.Value);
            return new SqlParameter("@proximaAplicacion", vacunacion.ProximaAplicacion.Value.Date);
        }

        List<BE.Vacunacion> Mapear(DataTable dt)
        {
            List<BE.Vacunacion> lista = new List<BE.Vacunacion>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Vacunacion vacunacion = new BE.Vacunacion();
                vacunacion.IdVacunacion = Convert.ToInt32(row["IdVacunacion"]);
                vacunacion.Nombre = row["Nombre"].ToString();
                vacunacion.Cantidad = Convert.ToInt32(row["Cantidad"]);
                vacunacion.IdCorral = Convert.ToInt32(row["IdCorral"]);
                vacunacion.NroCorral = row["NroCorral"].ToString();
                vacunacion.FechaAplicacion = Convert.ToDateTime(row["FechaAplicacion"]);
                if (row["ProximaAplicacion"] == DBNull.Value)
                    vacunacion.ProximaAplicacion = null;
                else
                    vacunacion.ProximaAplicacion = Convert.ToDateTime(row["ProximaAplicacion"]);
                lista.Add(vacunacion);
            }
            return lista;
        }
    }
}
