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
    public class MapperRaza : IMapper<BE.Raza>
    {
        public int Alta(BE.Raza raza)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombreRaza", raza.NombreRaza);
            return Acceso.Instancia.Escribir("AltaRaza", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idRaza", id);
            return Acceso.Instancia.Escribir("BajaRaza", parametros);
        }

        public int Modificar(BE.Raza raza)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idRaza", raza.IdRaza);
            parametros[1] = new SqlParameter("@nombreRaza", raza.NombreRaza);
            return Acceso.Instancia.Escribir("ModificarRaza", parametros);
        }

        public List<BE.Raza> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarRazas"));
        }

        // Busca por nombre (contiene el texto de filtro.NombreRaza).
        public List<BE.Raza> Buscar(BE.Raza filtro)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombreRaza", filtro.NombreRaza ?? "");
            return Mapear(Acceso.Instancia.Leer("BuscarRaza", parametros));
        }

        List<BE.Raza> Mapear(DataTable dt)
        {
            List<BE.Raza> lista = new List<BE.Raza>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Raza raza = new BE.Raza();
                raza.IdRaza = Convert.ToInt32(row["IdRaza"]);
                raza.NombreRaza = row["NombreRaza"].ToString();
                lista.Add(raza);
            }
            return lista;
        }
    }
}
