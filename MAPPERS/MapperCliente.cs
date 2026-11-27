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
    public class MapperCliente : IMapper<BE.Cliente>
    {
        public int Alta(BE.Cliente cliente)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@nombre", cliente.Nombre);
            parametros[1] = new SqlParameter("@apellido", cliente.Apellido);
            parametros[2] = new SqlParameter("@telefono", TextoONulo(cliente.Telefono));
            parametros[3] = new SqlParameter("@email", TextoONulo(cliente.Email));
            parametros[4] = new SqlParameter("@direccion", TextoONulo(cliente.Direccion));
            return Acceso.Instancia.Escribir("AltaCliente", parametros);
        }

        // Baja lógica: el cliente queda con Activo = 0 (sus ventas se conservan).
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idCliente", id);
            return Acceso.Instancia.Escribir("BajaCliente", parametros);
        }

        public int Modificar(BE.Cliente cliente)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("@idCliente", cliente.IdCliente);
            parametros[1] = new SqlParameter("@nombre", cliente.Nombre);
            parametros[2] = new SqlParameter("@apellido", cliente.Apellido);
            parametros[3] = new SqlParameter("@telefono", TextoONulo(cliente.Telefono));
            parametros[4] = new SqlParameter("@email", TextoONulo(cliente.Email));
            parametros[5] = new SqlParameter("@direccion", TextoONulo(cliente.Direccion));
            return Acceso.Instancia.Escribir("ModificarCliente", parametros);
        }

        // Solo clientes activos.
        public List<BE.Cliente> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarClientes"));
        }

        // Busca por nombre o apellido (el texto va en filtro.Nombre).
        public List<BE.Cliente> Buscar(BE.Cliente filtro)
        {
            string texto = filtro.Nombre;
            if (texto == null)
                texto = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@texto", texto);
            return Mapear(Acceso.Instancia.Leer("BuscarCliente", parametros));
        }

        // Los campos opcionales que vienen en null se mandan como NULL a la base.
        object TextoONulo(string texto)
        {
            if (texto == null)
                return DBNull.Value;
            return texto;
        }

        // Los campos opcionales que vienen NULL de la base quedan en null.
        string TextoDeFila(DataRow row, string columna)
        {
            if (row[columna] == DBNull.Value)
                return null;
            return row[columna].ToString();
        }

        List<BE.Cliente> Mapear(DataTable dt)
        {
            List<BE.Cliente> lista = new List<BE.Cliente>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Cliente cliente = new BE.Cliente();
                cliente.IdCliente = Convert.ToInt32(row["IdCliente"]);
                cliente.Nombre = row["Nombre"].ToString();
                cliente.Apellido = row["Apellido"].ToString();
                cliente.Telefono = TextoDeFila(row, "Telefono");
                cliente.Email = TextoDeFila(row, "Email");
                cliente.Direccion = TextoDeFila(row, "Direccion");
                cliente.Activo = Convert.ToBoolean(row["Activo"]);
                lista.Add(cliente);
            }
            return lista;
        }
    }
}
