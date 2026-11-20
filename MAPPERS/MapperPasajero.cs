using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAPPERS
{
    public class MapperPasajero : IMapper<BE.Pasajero>
    {
        DAL.Acceso acc = new DAL.Acceso();

        public int Alta(BE.Pasajero entidad)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@nombre", entidad.Nombre);
            parametros[1] = new SqlParameter("@apellido", entidad.Apellido);
            parametros[2] = new SqlParameter("@direccion", entidad.Direccion);
            return acc.Escribir("AltaPasajero", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idPasajero", id);
            return acc.Escribir("BajaPasajero", parametros);
        }

        public int Modificar(BE.Pasajero entidad)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idPasajero", entidad.IdPasajero);
            parametros[1] = new SqlParameter("@nombre", entidad.Nombre);
            parametros[2] = new SqlParameter("@apellido", entidad.Apellido);
            parametros[3] = new SqlParameter("@direccion", entidad.Direccion);
            return acc.Escribir("ModificarPasajero", parametros);
        }

        public List<BE.Pasajero> Listar()
        {
            return MapearLista(acc.Leer("ListarPasajeros"));
        }

        public List<BE.Pasajero> Buscar(BE.Pasajero filtro)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@apellido", filtro.Apellido);
            return MapearLista(acc.Leer("BuscarPasajero", parametros));
        }

        List<BE.Pasajero> MapearLista(DataTable dt)
        {
            List<BE.Pasajero> lista = new List<BE.Pasajero>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Pasajero p = new BE.Pasajero();
                p.IdPasajero = Convert.ToInt32(row["IdPasajero"]);
                p.Nombre = row["Nombre"].ToString();
                p.Apellido = row["Apellido"].ToString();
                p.Direccion = row["Direccion"].ToString();
                lista.Add(p);
            }
            return lista;
        }
    }
}
