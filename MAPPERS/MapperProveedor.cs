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
            parametros[1] = new SqlParameter("@telefono", TextoONulo(proveedor.Telefono));
            parametros[2] = new SqlParameter("@direccion", TextoONulo(proveedor.Direccion));
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
            parametros[2] = new SqlParameter("@telefono", TextoONulo(proveedor.Telefono));
            parametros[3] = new SqlParameter("@direccion", TextoONulo(proveedor.Direccion));
            return Acceso.Instancia.Escribir("ModificarProveedor", parametros);
        }

        public List<BE.Proveedor> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarProveedores"));
        }

        // Busca por nombre (contiene el texto de filtro.Nombre).
        public List<BE.Proveedor> Buscar(BE.Proveedor filtro)
        {
            string nombre = filtro.Nombre;
            if (nombre == null)
                nombre = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombre", nombre);
            return Mapear(Acceso.Instancia.Leer("BuscarProveedor", parametros));
        }

        // Los campos opcionales que vienen en null se mandan como NULL a la base.
        object TextoONulo(string texto)
        {
            if (texto == null)
                return DBNull.Value;
            return texto;
        }

        List<BE.Proveedor> Mapear(DataTable dt)
        {
            List<BE.Proveedor> lista = new List<BE.Proveedor>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Proveedor proveedor = new BE.Proveedor();
                proveedor.IdProveedor = Convert.ToInt32(row["IdProveedor"]);
                proveedor.Nombre = row["Nombre"].ToString();
                if (row["Telefono"] != DBNull.Value)
                    proveedor.Telefono = row["Telefono"].ToString();
                if (row["Direccion"] != DBNull.Value)
                    proveedor.Direccion = row["Direccion"].ToString();
                lista.Add(proveedor);
            }
            return lista;
        }
    }
}
