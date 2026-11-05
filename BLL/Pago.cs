using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Pago : IABMeable<BE.Pago>
    {
        DAL.MapPago mapper = new DAL.MapPago();
        public int Alta(BE.Pago entidad)
        {
            return mapper.AltaPago(entidad);
        }

        public int Baja(int id)
        {
            throw new NotImplementedException();
        }

        public List<BE.Pago> Buscar(BE.Pago filtro)
        {
            throw new NotImplementedException();
        }

        public List<BE.Pago> Listar()
        {
            throw new NotImplementedException();
        }

        public List<string> ListaPagados()
        {
            return mapper.ListaPagados();
            throw new NotImplementedException();
        }

        public string PromedioTotal()
        {
            return mapper.PromedioTotal();
            throw new NotImplementedException();
        }

        public int Modificar(BE.Pago entidad)
        {
            throw new NotImplementedException();
        }
    }
}
