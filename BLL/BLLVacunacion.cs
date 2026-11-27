using MAPPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio de las vacunaciones (se vacuna por corral).
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLVacunacion : IABMeable<BE.Vacunacion>
    {
        MapperVacunacion mapper = new MapperVacunacion();

        public int Alta(BE.Vacunacion vacunacion)
        {
            Validar(vacunacion);
            try
            {
                return mapper.Alta(vacunacion);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
                throw;
            }
        }

        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Vacunacion vacunacion)
        {
            Validar(vacunacion);
            try
            {
                return mapper.Modificar(vacunacion);
            }
            catch (SqlException ex)
            {
                TraducirError(ex);
                throw;
            }
        }

        public List<BE.Vacunacion> Listar()
        {
            return mapper.Listar();
        }

        public List<BE.Vacunacion> Buscar(BE.Vacunacion filtro)
        {
            return mapper.Buscar(filtro);
        }

        // Agenda: vacunaciones con próxima aplicación de hoy en adelante, la más cercana primero.
        public List<BE.Vacunacion> ListarProximas()
        {
            List<BE.Vacunacion> proximas = new List<BE.Vacunacion>();
            foreach (BE.Vacunacion vacunacion in mapper.Listar())
            {
                if (vacunacion.ProximaAplicacion != null && vacunacion.ProximaAplicacion.Value.Date >= DateTime.Today)
                {
                    // Se inserta ordenada por fecha de la próxima aplicación.
                    int posicion = 0;
                    while (posicion < proximas.Count && proximas[posicion].ProximaAplicacion.Value <= vacunacion.ProximaAplicacion.Value)
                        posicion++;
                    proximas.Insert(posicion, vacunacion);
                }
            }
            return proximas;
        }

        // 547 = el corral no existe (FK).
        void TraducirError(SqlException ex)
        {
            if (ex.Number == 547)
                throw new ArgumentException("El corral seleccionado no existe.");
        }

        void Validar(BE.Vacunacion vacunacion)
        {
            if (vacunacion == null)
                throw new ArgumentException("No hay datos de la vacunación.");

            if (vacunacion.Nombre == null)
                vacunacion.Nombre = "";
            vacunacion.Nombre = vacunacion.Nombre.Trim();
            if (vacunacion.Nombre == "")
                throw new ArgumentException("Ingrese el nombre de la vacuna.");
            if (vacunacion.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede tener más de 100 caracteres.");

            if (vacunacion.Cantidad <= 0)
                throw new ArgumentException("La cantidad tiene que ser mayor a cero.");
            if (vacunacion.IdCorral <= 0)
                throw new ArgumentException("Seleccione el corral.");

            if (vacunacion.FechaAplicacion == DateTime.MinValue)
                throw new ArgumentException("Ingrese la fecha de aplicación.");
            if (vacunacion.FechaAplicacion.Date > DateTime.Today)
                throw new ArgumentException("La fecha de aplicación no puede ser futura.");

            // La próxima aplicación es opcional.
            if (vacunacion.ProximaAplicacion != null && vacunacion.ProximaAplicacion.Value.Date < vacunacion.FechaAplicacion.Date)
                throw new ArgumentException("La próxima aplicación no puede ser anterior a la fecha de aplicación.");
        }
    }
}
