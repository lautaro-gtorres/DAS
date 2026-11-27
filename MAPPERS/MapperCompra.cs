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
    public class MapperCompra : IMapper<BE.Compra>
    {
        // El SP también suma los kilos al stock del alimento.
        public int Alta(BE.Compra compra)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@fecha", compra.Fecha.Date);
            parametros[1] = new SqlParameter("@idAlimento", compra.IdAlimento);
            parametros[2] = new SqlParameter("@idProveedor", compra.IdProveedor);
            parametros[3] = new SqlParameter("@kilogramos", compra.Kilogramos);
            parametros[4] = new SqlParameter("@precioUnitario", compra.PrecioUnitario);
            return Acceso.Instancia.Escribir("AltaCompra", parametros);
        }

        // Anula la compra y resta los kilos del stock.
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idCompra", id);
            return Acceso.Instancia.Escribir("BajaCompra", parametros);
        }

        // Solo fecha y precio.
        public int Modificar(BE.Compra compra)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idCompra", compra.IdCompra);
            parametros[1] = new SqlParameter("@fecha", compra.Fecha.Date);
            parametros[2] = new SqlParameter("@precioUnitario", compra.PrecioUnitario);
            return Acceso.Instancia.Escribir("ModificarCompra", parametros);
        }

        public List<BE.Compra> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarCompras"));
        }

        // Busca por nombre del proveedor (el texto va en filtro.Proveedor).
        public List<BE.Compra> Buscar(BE.Compra filtro)
        {
            string proveedor = filtro.Proveedor;
            if (proveedor == null)
                proveedor = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@proveedor", proveedor);
            return Mapear(Acceso.Instancia.Leer("BuscarCompra", parametros));
        }

        List<BE.Compra> Mapear(DataTable dt)
        {
            List<BE.Compra> lista = new List<BE.Compra>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Compra compra = new BE.Compra();
                compra.IdCompra = Convert.ToInt32(row["IdCompra"]);
                compra.Fecha = Convert.ToDateTime(row["Fecha"]);
                compra.IdAlimento = Convert.ToInt32(row["IdAlimento"]);
                compra.Alimento = row["Alimento"].ToString();
                compra.IdProveedor = Convert.ToInt32(row["IdProveedor"]);
                compra.Proveedor = row["Proveedor"].ToString();
                compra.Kilogramos = Convert.ToDecimal(row["Kilogramos"]);
                compra.PrecioUnitario = Convert.ToDecimal(row["PrecioUnitario"]);
                compra.Total = Convert.ToDecimal(row["Total"]);
                lista.Add(compra);
            }
            return lista;
        }
    }
}
