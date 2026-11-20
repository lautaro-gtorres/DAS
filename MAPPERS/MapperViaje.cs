using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    public class MapperViaje
    {
        DAL.Acceso acc = new DAL.Acceso();

        public int Alta(BE.Viaje viaje)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("@idVehiculo", viaje.IdVehiculo);
            parametros[1] = new SqlParameter("@idPasajero", viaje.IdPasajero);
            parametros[2] = new SqlParameter("@fecha", viaje.Fecha);
            parametros[3] = new SqlParameter("@destino", viaje.Destino);
            parametros[4] = new SqlParameter("@kilometro", viaje.Kilometro);
            parametros[5] = new SqlParameter("@importe", viaje.Importe);
            return acc.Escribir("AltaViaje", parametros);
        }

        public List<BE.Viaje> Listar()
        {
            List<BE.Viaje> lista = new List<BE.Viaje>();
            foreach (DataRow row in acc.Leer("ListarViajes").Rows)
            {
                BE.Viaje v = new BE.Viaje();
                v.IdViaje = Convert.ToInt32(row["IdViaje"]);
                v.IdVehiculo = Convert.ToInt32(row["IdVehiculo"]);
                v.IdPasajero = Convert.ToInt32(row["IdPasajero"]);
                v.Fecha = Convert.ToDateTime(row["Fecha"]);
                v.Destino = row["Destino"].ToString();
                v.Kilometro = Convert.ToDecimal(row["Kilometro"]);
                v.Importe = Convert.ToDecimal(row["Importe"]);
                lista.Add(v);
            }
            return lista;
        }

        public void GuardarXml(List<BE.Viaje> viajes, string ruta)
        {
            acc.EscribirXml(viajes, ruta);
        }

        public List<BE.Viaje> LeerXml(string ruta)
        {
            return acc.LeerXml<BE.Viaje>(ruta);
        }
    }
}
