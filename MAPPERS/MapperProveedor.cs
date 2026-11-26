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
    public class MapperProveedor : IMapper<BE.Proveedor>
    {
        public int Alta(BE.Proveedor proveedor)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@nombre", proveedor.Nombre);
            parametros[1] = new SqlParameter("@telefono", (object)proveedor.Telefono ?? DBNull.Value);
            parametros[2] = new SqlParameter("@direccion", (object)proveedor.Direccion ?? DBNull.Value);
            return Acceso.Instancia.Escribir("AltaProveedor", parametros);
        }

        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idProveedor", id);
            return Acceso.Instancia.Escribir("BajaProveedor", parametros);
        }

        public int Modificar(BE.Proveedor proveedor)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idProveedor", proveedor.IdProveedor);
            parametros[1] = new SqlParameter("@nombre", proveedor.Nombre);
            parametros[2] = new SqlParameter("@telefono", (object)proveedor.Telefono ?? DBNull.Value);
            parametros[3] = new SqlParameter("@direccion", (object)proveedor.Direccion ?? DBNull.Value);
            return Acceso.Instancia.Escribir("ModificarProveedor", parametros);
        }

        public List<BE.Proveedor> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarProveedores"));
        }

        // Busca por nombre (contiene el texto de filtro.Nombre).
        public List<BE.Proveedor> Buscar(BE.Proveedor filtro)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombre", filtro.Nombre ?? "");
            return Mapear(Acceso.Instancia.Leer("BuscarProveedor", parametros));
        }

        List<BE.Proveedor> Mapear(DataTable dt)
        {
            List<BE.Proveedor> lista = new List<BE.Proveedor>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Proveedor proveedor = new BE.Proveedor();
                proveedor.IdProveedor = Convert.ToInt32(row["IdProveedor"]);
                proveedor.Nombre = row["Nombre"].ToString();
                proveedor.Telefono = row["Telefono"] == DBNull.Value ? null : row["Telefono"].ToString();
                proveedor.Direccion = row["Direccion"] == DBNull.Value ? null : row["Direccion"].ToString();
                lista.Add(proveedor);
            }
            return lista;
        }
    }
}
