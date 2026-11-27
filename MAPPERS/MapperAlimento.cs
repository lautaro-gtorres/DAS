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
    public class MapperAlimento : IMapper<BE.Alimento>
    {
        public int Alta(BE.Alimento alimento)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@nombreAlimento", alimento.NombreAlimento);
            parametros[1] = new SqlParameter("@kilogramos", alimento.Kilogramos);
            parametros[2] = new SqlParameter("@idProveedor", alimento.IdProveedor);
            return Acceso.Instancia.Escribir("AltaAlimento", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idAlimento", id);
            return Acceso.Instancia.Escribir("BajaAlimento", parametros);
        }

        public int Modificar(BE.Alimento alimento)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idAlimento", alimento.IdAlimento);
            parametros[1] = new SqlParameter("@nombreAlimento", alimento.NombreAlimento);
            parametros[2] = new SqlParameter("@kilogramos", alimento.Kilogramos);
            parametros[3] = new SqlParameter("@idProveedor", alimento.IdProveedor);
            return Acceso.Instancia.Escribir("ModificarAlimento", parametros);
        }

        public List<BE.Alimento> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarAlimentos"));
        }

        // Busca por nombre (contiene el texto de filtro.NombreAlimento).
        public List<BE.Alimento> Buscar(BE.Alimento filtro)
        {
            string nombre = filtro.NombreAlimento;
            if (nombre == null)
                nombre = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombreAlimento", nombre);
            return Mapear(Acceso.Instancia.Leer("BuscarAlimento", parametros));
        }

        List<BE.Alimento> Mapear(DataTable dt)
        {
            List<BE.Alimento> lista = new List<BE.Alimento>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Alimento alimento = new BE.Alimento();
                alimento.IdAlimento = Convert.ToInt32(row["IdAlimento"]);
                alimento.NombreAlimento = row["NombreAlimento"].ToString();
                alimento.Kilogramos = Convert.ToDecimal(row["Kilogramos"]);
                alimento.IdProveedor = Convert.ToInt32(row["IdProveedor"]);
                alimento.Proveedor = row["Proveedor"].ToString();
                lista.Add(alimento);
            }
            return lista;
        }
    }
}
