using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPasajero : IABMeable<BE.Pasajero>
    {
        MAPPERS.MapperPasajero mapper = new MAPPERS.MapperPasajero();

        public int Alta(BE.Pasajero entidad)
        {
            Validar(entidad);
            return mapper.Alta(entidad);
        }

        public int Baja(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Seleccioná un pasajero.");
            try
            {
                return mapper.Baja(id);
            }
            catch (SqlException ex) when (ex.Number == 547)   // FK: tiene viajes
            {
                throw new ArgumentException("No se puede dar de baja: el pasajero tiene viajes registrados.");
            }
        }

        public int Modificar(BE.Pasajero entidad)
        {
            if (entidad.IdPasajero <= 0)
                throw new ArgumentException("Seleccioná un pasajero.");
            Validar(entidad);
            return mapper.Modificar(entidad);
        }

        public List<BE.Pasajero> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Pasajero> Buscar(BE.Pasajero filtro)
        {
            return mapper.Buscar(filtro);
        }

        void Validar(BE.Pasajero p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre) || string.IsNullOrWhiteSpace(p.Apellido))
                throw new ArgumentException("Nombre y apellido son obligatorios.");
            if (!(p.Nombre + p.Apellido).All(c => char.IsLetter(c) || c == ' '))
                throw new ArgumentException("Nombre y apellido solo pueden tener letras.");
            if (string.IsNullOrWhiteSpace(p.Direccion))
                throw new ArgumentException("La dirección es obligatoria.");
        }
    }
}
