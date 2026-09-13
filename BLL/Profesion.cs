using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Profesion : IABMeable<BE.Profesion>
    {
        DAL.MapProfesion mapper = new DAL.MapProfesion();

        void Validar(BE.Profesion entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.DescProfesion))
                throw new ArgumentException("La profesión no puede estar vacía.");
        }

        public int Alta(BE.Profesion entidad)
        {
            Validar(entidad);
            return mapper.AltaProfesion(entidad);
        }

        public int Baja(int id)
        {
            return mapper.BajaProfesion(id);
        }

        public List<BE.Profesion> Buscar(BE.Profesion filtro)
        {
            return mapper.BuscarProfesion(filtro);
        }

        public List<BE.Profesion> Listar()
        {
            return mapper.ListarProfesion();
        }

        public int Modificar(BE.Profesion entidad)
        {
            Validar(entidad);
            return mapper.ModificarProfesion(entidad);
        }
    }
}
