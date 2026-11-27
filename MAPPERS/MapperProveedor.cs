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
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("@nombre", proveedor.Nombre);
            parametros[1] = new SqlParameter("@cuil", Conversion.TextoONulo(proveedor.Cuil));
            parametros[2] = new SqlParameter("@direccion", Conversion.TextoONulo(proveedor.Direccion));
            parametros[3] = new SqlParameter("@localidad", Conversion.TextoONulo(proveedor.Localidad));
            parametros[4] = new SqlParameter("@telefono", Conversion.TextoONulo(proveedor.Telefono));
            parametros[5] = new SqlParameter("@email", Conversion.TextoONulo(proveedor.Email));
            parametros[6] = new SqlParameter("@servicioProducto", Conversion.TextoONulo(proveedor.ServicioProducto));
            parametros[7] = new SqlParameter("@activo", proveedor.Activo);
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
            SqlParameter[] parametros = new SqlParameter[9];
            parametros[0] = new SqlParameter("@idProveedor", proveedor.IdProveedor);
            parametros[1] = new SqlParameter("@nombre", proveedor.Nombre);
            parametros[2] = new SqlParameter("@cuil", Conversion.TextoONulo(proveedor.Cuil));
            parametros[3] = new SqlParameter("@direccion", Conversion.TextoONulo(proveedor.Direccion));
            parametros[4] = new SqlParameter("@localidad", Conversion.TextoONulo(proveedor.Localidad));
            parametros[5] = new SqlParameter("@telefono", Conversion.TextoONulo(proveedor.Telefono));
            parametros[6] = new SqlParameter("@email", Conversion.TextoONulo(proveedor.Email));
            parametros[7] = new SqlParameter("@servicioProducto", Conversion.TextoONulo(proveedor.ServicioProducto));
            parametros[8] = new SqlParameter("@activo", proveedor.Activo);
            return Acceso.Instancia.Escribir("ModificarProveedor", parametros);
        }

        // Activos e inactivos.
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

        List<BE.Proveedor> Mapear(DataTable dt)
        {
            List<BE.Proveedor> lista = new List<BE.Proveedor>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Proveedor proveedor = new BE.Proveedor();
                proveedor.IdProveedor = Convert.ToInt32(row["IdProveedor"]);
                proveedor.Nombre = row["Nombre"].ToString();
                proveedor.Cuil = Conversion.TextoDeFila(row, "CUIL");
                proveedor.Direccion = Conversion.TextoDeFila(row, "Direccion");
                proveedor.Localidad = Conversion.TextoDeFila(row, "Localidad");
                proveedor.Telefono = Conversion.TextoDeFila(row, "Telefono");
                proveedor.Email = Conversion.TextoDeFila(row, "Email");
                proveedor.ServicioProducto = Conversion.TextoDeFila(row, "ServicioProducto");
                proveedor.Activo = Convert.ToBoolean(row["Activo"]);
                lista.Add(proveedor);
            }
            return lista;
        }
    }
}
