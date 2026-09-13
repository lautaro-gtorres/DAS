using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Persona : IABMeable<BE.Persona>
    {
        DAL.MapPersona mapper = new DAL.MapPersona();

        void Validar(BE.Persona entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(entidad.Apellido))
                throw new ArgumentException("El apellido es obligatorio.");
            if (entidad.Edad <= 0 || entidad.Edad > 120)
                throw new ArgumentException("La edad debe estar entre 1 y 120.");
            if (entidad.Sexo != "M" && entidad.Sexo != "F")
                throw new ArgumentException("El sexo debe ser M o F.");
            if (entidad.Idnacionalidad <= 0)
                throw new ArgumentException("Debe seleccionar una nacionalidad.");
            if (entidad.IdProfesion <= 0)
                throw new ArgumentException("Debe seleccionar una profesión.");
        }

        public int Alta(BE.Persona entidad)
        {
            Validar(entidad);
            return mapper.AltaPersona(entidad);
        }

        public int Baja(int id)
        {
           return mapper.BajaPersona(id);
        }
        public List<BE.Persona> Listar()
        {

            return mapper.ListarPersona();
        }
        public int Modificar(BE.Persona entidad)
        {
            Validar(entidad);
            return mapper.ModificarPersona(entidad);
        }
        public List<BE.Persona> Buscar(BE.Persona filtro)
        {
            return mapper.BuscarPersona(filtro);
        }
    }
}
