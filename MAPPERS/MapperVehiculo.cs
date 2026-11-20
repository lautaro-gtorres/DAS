using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    public class MapperVehiculo
    {
        DAL.Acceso acc = new DAL.Acceso();

        public List<BE.Vehiculo> Listar()
        {
            List<BE.Vehiculo> lista = new List<BE.Vehiculo>();
            foreach (DataRow row in acc.Leer("ListarVehiculos").Rows)
            {
                BE.Vehiculo v = new BE.Vehiculo();
                v.IdVehiculo = Convert.ToInt32(row["IdVehiculo"]);
                v.Marca = row["Marca"].ToString();
                v.Modelo = row["Modelo"].ToString();
                v.Patente = row["Patente"].ToString();
                v.Disponible = Convert.ToBoolean(row["Disponible"]);
                lista.Add(v);
            }
            return lista;
        }

        public int Liberar(int idVehiculo)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idVehiculo", idVehiculo);
            return acc.Escribir("LiberarVehiculo", parametros);
        }
    }
}
