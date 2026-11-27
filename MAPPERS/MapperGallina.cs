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
    public class MapperGallina : IMapper<BE.Gallina>
    {
        public int Alta(BE.Gallina gallina)
        {
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("@codGallina", gallina.CodGallina);
            parametros[1] = new SqlParameter("@idRaza", gallina.IdRaza);
            parametros[2] = new SqlParameter("@idTipoGallina", gallina.IdTipoGallina);
            parametros[3] = new SqlParameter("@sexo", gallina.Sexo.ToString());
            parametros[4] = ParametroFechaNacimiento(gallina);
            parametros[5] = new SqlParameter("@idCorral", gallina.IdCorral);
            parametros[6] = ParametroDetalle(gallina);
            return Acceso.Instancia.Escribir("AltaGallina", parametros);
        }

        // Carga por mayor: da de alta todas las gallinas en una sola transacción
        // (si una falla, por ejemplo por código repetido, no se guarda ninguna).
        public int AltaVarias(List<BE.Gallina> gallinas)
        {
            int filas = 0;
            Acceso.Instancia.IniciarTransaccion();
            try
            {
                foreach (BE.Gallina gallina in gallinas)
                {
                    SqlParameter[] parametros = new SqlParameter[7];
                    parametros[0] = new SqlParameter("@codGallina", gallina.CodGallina);
                    parametros[1] = new SqlParameter("@idRaza", gallina.IdRaza);
                    parametros[2] = new SqlParameter("@idTipoGallina", gallina.IdTipoGallina);
                    parametros[3] = new SqlParameter("@sexo", gallina.Sexo.ToString());
                    parametros[4] = ParametroFechaNacimiento(gallina);
                    parametros[5] = new SqlParameter("@idCorral", gallina.IdCorral);
                    parametros[6] = ParametroDetalle(gallina);
                    filas = filas + Acceso.Instancia.EscribirEnTransaccion("AltaGallina", parametros);
                }
                Acceso.Instancia.ConfirmarTransaccion();
            }
            catch (Exception)
            {
                Acceso.Instancia.CancelarTransaccion();
                throw;
            }
            return filas;
        }

        // Baja lógica: la gallina queda con Activo = 0.
        public int Baja(int id)
        {
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idGallina", id);
            return Acceso.Instancia.Escribir("BajaGallina", parametros);
        }

        public int Modificar(BE.Gallina gallina)
        {
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("@idGallina", gallina.IdGallina);
            parametros[1] = new SqlParameter("@codGallina", gallina.CodGallina);
            parametros[2] = new SqlParameter("@idRaza", gallina.IdRaza);
            parametros[3] = new SqlParameter("@idTipoGallina", gallina.IdTipoGallina);
            parametros[4] = new SqlParameter("@sexo", gallina.Sexo.ToString());
            parametros[5] = ParametroFechaNacimiento(gallina);
            parametros[6] = new SqlParameter("@idCorral", gallina.IdCorral);
            parametros[7] = ParametroDetalle(gallina);
            return Acceso.Instancia.Escribir("ModificarGallina", parametros);
        }

        // Solo gallinas activas.
        public List<BE.Gallina> Listar()
        {
            return Mapear(Acceso.Instancia.Leer("ListarGallinas"));
        }

        // Busca por código (contiene el texto de filtro.CodGallina).
        public List<BE.Gallina> Buscar(BE.Gallina filtro)
        {
            string codigo = filtro.CodGallina;
            if (codigo == null)
                codigo = "";

            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@codGallina", codigo);
            return Mapear(Acceso.Instancia.Leer("BuscarGallina", parametros));
        }

        // Catálogo para el ComboBox de tipo de gallina.
        public List<BE.TipoGallina> ListarTipos()
        {
            List<BE.TipoGallina> lista = new List<BE.TipoGallina>();
            DataTable dt = Acceso.Instancia.Leer("ListarTiposGallina");
            foreach (DataRow row in dt.Rows)
            {
                BE.TipoGallina tipo = new BE.TipoGallina();
                tipo.IdTipoGallina = Convert.ToInt32(row["IdTipoGallina"]);
                tipo.Descripcion = row["Descripcion"].ToString();
                lista.Add(tipo);
            }
            return lista;
        }

        // La fecha de nacimiento es opcional: si no tiene valor se manda NULL.
        SqlParameter ParametroFechaNacimiento(BE.Gallina gallina)
        {
            SqlParameter parametro = new SqlParameter("@fechaNacimiento", SqlDbType.Date);
            if (gallina.FechaNacimiento == null)
                parametro.Value = DBNull.Value;
            else
                parametro.Value = gallina.FechaNacimiento.Value;
            return parametro;
        }

        // El detalle es opcional: si no tiene valor se manda NULL.
        SqlParameter ParametroDetalle(BE.Gallina gallina)
        {
            if (gallina.Detalle == null)
                return new SqlParameter("@detalle", DBNull.Value);
            return new SqlParameter("@detalle", gallina.Detalle);
        }

        List<BE.Gallina> Mapear(DataTable dt)
        {
            List<BE.Gallina> lista = new List<BE.Gallina>();
            foreach (DataRow row in dt.Rows)
            {
                BE.Gallina gallina = new BE.Gallina();
                gallina.IdGallina = Convert.ToInt32(row["IdGallina"]);
                if (row["CodGallina"] != DBNull.Value)
                    gallina.CodGallina = row["CodGallina"].ToString();
                gallina.IdRaza = Convert.ToInt32(row["IdRaza"]);
                gallina.Raza = row["Raza"].ToString();
                gallina.IdTipoGallina = Convert.ToInt32(row["IdTipoGallina"]);
                gallina.TipoGallina = row["TipoGallina"].ToString();
                gallina.Sexo = Convert.ToChar(row["Sexo"]);
                if (row["FechaNacimiento"] != DBNull.Value)
                    gallina.FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]);
                gallina.IdCorral = Convert.ToInt32(row["IdCorral"]);
                gallina.NroCorral = row["NroCorral"].ToString();
                if (row["Detalle"] != DBNull.Value)
                    gallina.Detalle = row["Detalle"].ToString();
                gallina.Activo = Convert.ToBoolean(row["Activo"]);
                lista.Add(gallina);
            }
            return lista;
        }
    }
}
