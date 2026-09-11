using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class MapResumen
    {
        Acceso acc = new Acceso();

        public BE.ResumenGeneral ObtenerResumenGeneral()
        {
            DataTable dt = acc.Leer("ResumenGeneral");
            DataRow row = dt.Rows[0];
            return new BE.ResumenGeneral
            {
                Cantidad = Convert.ToInt32(row["Cantidad"]),
                PromedioEdad = row["PromedioEdad"] == DBNull.Value ? 0 : Convert.ToDouble(row["PromedioEdad"]),
                EdadMinima = row["EdadMinima"] == DBNull.Value ? 0 : Convert.ToInt32(row["EdadMinima"]),
                EdadMaxima = row["EdadMaxima"] == DBNull.Value ? 0 : Convert.ToInt32(row["EdadMaxima"])
            };
        }

        public List<BE.ResumenNacionalidad> ObtenerResumenPorNacionalidad()
        {
            List<BE.ResumenNacionalidad> lista = new List<BE.ResumenNacionalidad>();
            DataTable dt = acc.Leer("ResumenPorNacionalidad");
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new BE.ResumenNacionalidad
                {
                    IdNacionalidad = Convert.ToInt32(row["idNacionalidad"]),
                    DescNacionalidad = row["descNacionalidad"].ToString(),
                    Cantidad = Convert.ToInt32(row["Cantidad"]),
                    PromedioEdad = row["PromedioEdad"] == DBNull.Value ? 0 : Convert.ToDouble(row["PromedioEdad"])
                });
            }
            return lista;
        }

        public List<BE.ResumenProfesion> ObtenerResumenPorProfesion()
        {
            List<BE.ResumenProfesion> lista = new List<BE.ResumenProfesion>();
            DataTable dt = acc.Leer("ResumenPorTipoProfesion");
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new BE.ResumenProfesion
                {
                    IdProfesion = Convert.ToInt32(row["idProfesion"]),
                    DescProfesion = row["descProfesion"].ToString(),
                    Cantidad = Convert.ToInt32(row["Cantidad"]),
                    PromedioEdad = row["PromedioEdad"] == DBNull.Value ? 0 : Convert.ToDouble(row["PromedioEdad"])
                });
            }
            return lista;
        }
    }
}
