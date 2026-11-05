using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MapUbicacion
    {
        Acceso acc = new Acceso();

        public List<BE.Ubicacion> ListarUbicaciones()
        {
            List<BE.Ubicacion> ubicaciones = new List<BE.Ubicacion>();
            var dt = acc.Leer("ListarUbicacion");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                BE.Ubicacion ubicacion = new BE.Ubicacion(
                    row["IdUbicacion"].ToString(),
                    row["Descripcion"].ToString(),
                    Convert.ToInt32(row["ValorMt2"])
                );
                ubicaciones.Add(ubicacion);
            }
            return ubicaciones;
        }
    }
}
