using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLViaje
    {
        MAPPERS.MapperViaje mapper = new MAPPERS.MapperViaje();

        // $9500 cada 30 km (o fracción): 1-30 km = 9500, 31-60 = 19000, ...
        public decimal CalcularImporte(decimal km)
        {
            return Math.Ceiling(km / 30) * 9500;
        }

        public int Alta(BE.Viaje viaje)
        {
            if (viaje.IdPasajero <= 0)
                throw new ArgumentException("Seleccioná un pasajero.");
            if (viaje.IdVehiculo <= 0)
                throw new ArgumentException("Seleccioná un vehículo disponible.");
            if (string.IsNullOrWhiteSpace(viaje.Destino))
                throw new ArgumentException("El destino es obligatorio.");
            if (viaje.Kilometro <= 0)
                throw new ArgumentException("Los kilómetros deben ser mayores a 0.");

            viaje.Importe = CalcularImporte(viaje.Kilometro);
            return mapper.Alta(viaje);
        }

        public List<BE.Viaje> Listar()
        {
            return mapper.Listar();
        }

        // Punto 4
        public int Cantidad()
        {
            return mapper.Listar().Count;
        }

        // Punto 5
        public decimal Total()
        {
            return mapper.Listar().Sum(v => v.Importe);
        }

        // Punto 6
        public List<BE.Viaje> GuardarXmlMayores25000(string ruta)
        {
            List<BE.Viaje> viajes = mapper.Listar().Where(v => v.Importe > 25000).ToList();
            mapper.GuardarXml(viajes, ruta);
            return viajes;
        }

        // Punto 7
        public List<BE.Viaje> GuardarXmlHasta20Km(string ruta)
        {
            List<BE.Viaje> viajes = mapper.Listar().Where(v => v.Kilometro <= 20).ToList();
            mapper.GuardarXml(viajes, ruta);
            return viajes;
        }

        public List<BE.Viaje> LeerXml(string ruta)
        {
            return mapper.LeerXml(ruta);
        }

        // Punto 8
        public List<BE.Viaje> ListarPorVehiculo(int idVehiculo)
        {
            return mapper.Listar().Where(v => v.IdVehiculo == idVehiculo).ToList();
        }
    }
}
