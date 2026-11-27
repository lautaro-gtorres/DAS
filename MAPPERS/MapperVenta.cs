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
    public class MapperVenta : IMapper<BE.Venta>
    {
        // Si son gallinas, el SP también las da de baja (todo en la misma transacción).
        // Al terminar, venta.IdVenta queda con el número de la venta nueva (parámetro OUTPUT).
        public int Alta(BE.Venta venta)
        {
            SqlParameter[] parametros = new SqlParameter[9];
            parametros[0] = new SqlParameter("@fecha", venta.Fecha.Date);
            parametros[1] = new SqlParameter("@idTipoProducto", venta.IdTipoProducto);
            parametros[2] = new SqlParameter("@cantidad", venta.Cantidad);
            parametros[3] = new SqlParameter("@precioUnitario", venta.PrecioUnitario);
            parametros[4] = NumeroONulo("@idCliente", venta.IdCliente);
            parametros[5] = NumeroONulo("@idTipoGallina", venta.IdTipoGallina);
            parametros[6] = NumeroONulo("@idRaza", venta.IdRaza);
            if (venta.Sexo == null)
                parametros[7] = new SqlParameter("@sexo", DBNull.Value);
            else
                parametros[7] = new SqlParameter("@sexo", venta.Sexo.Value.ToString());
            parametros[8] = new SqlParameter("@idVenta", SqlDbType.Int);
            parametros[8].Direction = ParameterDirection.Output;

            int filas = Acceso.Instancia.Escribir("AltaVenta", parametros);
            venta.IdVenta = Convert.ToInt32(parametros[8].Value);
            return filas;
        }

        // ---- Comprobante ----

        // Datos del comprobante de una venta, o null si la venta no existe.
        public BE.Comprobante ObtenerComprobante(int idVenta)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idVenta", idVenta);
            DataTable dt = Acceso.Instancia.Leer("ComprobanteVenta", parametros);
            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            BE.Comprobante comprobante = new BE.Comprobante();
            comprobante.IdVenta = Convert.ToInt32(row["IdVenta"]);
            comprobante.NroComprobante = row["NroComprobante"].ToString();
            comprobante.Fecha = Convert.ToDateTime(row["Fecha"]);
            comprobante.Cliente = row["Cliente"].ToString();
            comprobante.Descripcion = row["Descripcion"].ToString();
            comprobante.Cantidad = Convert.ToInt32(row["Cantidad"]);
            comprobante.PrecioUnitario = Convert.ToDecimal(row["PrecioUnitario"]);
            comprobante.Total = Convert.ToDecimal(row["Total"]);
            return comprobante;
        }

        // Guarda el comprobante de la venta en un archivo XML.
        public void ExportarComprobanteXML(int idVenta, string ruta)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idVenta", idVenta);
            Acceso.Instancia.ExportarXML("ComprobanteVenta", parametros, "Comprobante", ruta);
        }

        // Anula la venta: las gallinas vendidas vuelven a estar activas.
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idVenta", id);
            return Acceso.Instancia.Escribir("BajaVenta", parametros);
        }

        // Solo cambia fecha, cliente y precio.
        public int Modificar(BE.Venta venta)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@idVenta", venta.IdVenta);
            parametros[1] = new SqlParameter("@fecha", venta.Fecha.Date);
            parametros[2] = NumeroONulo("@idCliente", venta.IdCliente);
            parametros[3] = new SqlParameter("@precioUnitario", venta.PrecioUnitario);
            return Acceso.Instancia.Escribir("ModificarVenta", parametros);
        }

        public List<BE.Venta> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarVentas"));
        }

        // Filtra por día (filtro.Fecha), producto (filtro.IdTipoProducto) y cliente (filtro.IdCliente).
        // Lo que no se completa no filtra.
        public List<BE.Venta> Buscar(BE.Venta filtro)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            if (filtro.Fecha == DateTime.MinValue)
                parametros[0] = new SqlParameter("@fecha", DBNull.Value);
            else
                parametros[0] = new SqlParameter("@fecha", filtro.Fecha.Date);
            if (filtro.IdTipoProducto <= 0)
                parametros[1] = new SqlParameter("@idTipoProducto", DBNull.Value);
            else
                parametros[1] = new SqlParameter("@idTipoProducto", filtro.IdTipoProducto);
            parametros[2] = NumeroONulo("@idCliente", filtro.IdCliente);
            return Mapear(Acceso.Instancia.Leer("BuscarVenta", parametros));
        }

        // Catálogo para el selector de producto (Huevo / Gallina).
        public List<BE.TipoProducto> ListarTiposProducto()
        {
            List<BE.TipoProducto> lista = new List<BE.TipoProducto>();
            DataTable dt = Acceso.Instancia.Leer("ListarTiposProducto");
            foreach (DataRow row in dt.Rows)
            {
                BE.TipoProducto tipo = new BE.TipoProducto();
                tipo.IdTipoProducto = Convert.ToInt32(row["IdTipoProducto"]);
                tipo.Descripcion = row["Descripcion"].ToString();
                lista.Add(tipo);
            }
            return lista;
        }

        // Cuántas gallinas activas hay de ese tipo, raza y sexo.
        public int ContarGallinasDisponibles(int idTipoGallina, int idRaza, char sexo)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idTipoGallina", idTipoGallina);
            parametros[1] = new SqlParameter("@idRaza", idRaza);
            parametros[2] = new SqlParameter("@sexo", sexo.ToString());
            DataTable dt = Acceso.Instancia.Leer("ContarGallinasDisponibles", parametros);
            return Convert.ToInt32(dt.Rows[0]["Disponibles"]);
        }

        // Los valores opcionales que vienen en null se mandan como NULL a la base.
        SqlParameter NumeroONulo(string nombre, int? valor)
        {
            if (valor == null)
                return new SqlParameter(nombre, DBNull.Value);
            return new SqlParameter(nombre, valor.Value);
        }

        int? NumeroDeFila(DataRow row, string columna)
        {
            if (row[columna] == DBNull.Value)
                return null;
            return Convert.ToInt32(row[columna]);
        }

        string TextoDeFila(DataRow row, string columna)
        {
            if (row[columna] == DBNull.Value)
                return null;
            return row[columna].ToString();
        }

        List<BE.Venta> Mapear(DataTable dt)
        {
            List<BE.Venta> lista = new List<BE.Venta>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Venta venta = new BE.Venta();
                venta.IdVenta = Convert.ToInt32(row["IdVenta"]);
                venta.Fecha = Convert.ToDateTime(row["Fecha"]);
                venta.IdTipoProducto = Convert.ToInt32(row["IdTipoProducto"]);
                venta.TipoProducto = row["TipoProducto"].ToString();
                venta.Cantidad = Convert.ToInt32(row["Cantidad"]);
                venta.PrecioUnitario = Convert.ToDecimal(row["PrecioUnitario"]);
                venta.Total = Convert.ToDecimal(row["Total"]);
                venta.IdCliente = NumeroDeFila(row, "IdCliente");
                venta.Cliente = TextoDeFila(row, "Cliente");
                venta.IdTipoGallina = NumeroDeFila(row, "IdTipoGallina");
                venta.TipoGallina = TextoDeFila(row, "TipoGallina");
                venta.IdRaza = NumeroDeFila(row, "IdRaza");
                venta.Raza = TextoDeFila(row, "Raza");
                if (row["Sexo"] == DBNull.Value)
                    venta.Sexo = null;
                else
                    venta.Sexo = row["Sexo"].ToString()[0];
                lista.Add(venta);
            }
            return lista;
        }
    }
}
