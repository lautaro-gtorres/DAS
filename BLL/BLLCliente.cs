using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Reglas de negocio del ABM de clientes (baja lógica).
    // Si un dato no es válido lanza ArgumentException con un mensaje para mostrar al usuario.
    public class BLLCliente : IABMeable<BE.Cliente>
    {
        MapperCliente mapper = new MapperCliente();

        public int Alta(BE.Cliente cliente)
        {
            Validar(cliente);
            return mapper.Alta(cliente);
        }

        // Baja lógica: el cliente deja de aparecer en los listados, pero sus ventas se conservan.
        public int Baja(int id)
        {
            return mapper.Baja(id);
        }

        public int Modificar(BE.Cliente cliente)
        {
            Validar(cliente);
            return mapper.Modificar(cliente);
        }

        public List<BE.Cliente> Listar()
        {
            return mapper.Listar();
        }

        // Busca por nombre o apellido (el texto va en filtro.Nombre).
        public List<BE.Cliente> Buscar(BE.Cliente filtro)
        {
            return mapper.Buscar(filtro);
        }

        void Validar(BE.Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentException("No hay datos del cliente.");

            cliente.Nombre = Validaciones.SacarEspacios(cliente.Nombre);
            if (cliente.Nombre == null)
                throw new ArgumentException("Ingrese el nombre del cliente.");
            if (cliente.Nombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener más de 50 caracteres.");

            cliente.Apellido = Validaciones.SacarEspacios(cliente.Apellido);
            if (cliente.Apellido == null)
                throw new ArgumentException("Ingrese el apellido del cliente.");
            if (cliente.Apellido.Length > 50)
                throw new ArgumentException("El apellido no puede tener más de 50 caracteres.");

            // Teléfono, e-mail y dirección son opcionales: si vienen vacíos se guardan como NULL.
            cliente.Telefono = Validaciones.SacarEspacios(cliente.Telefono);
            if (cliente.Telefono != null && cliente.Telefono.Length > 20)
                throw new ArgumentException("El teléfono no puede tener más de 20 caracteres.");

            cliente.Email = Validaciones.SacarEspacios(cliente.Email);
            if (cliente.Email != null)
            {
                if (cliente.Email.Length > 100)
                    throw new ArgumentException("El e-mail no puede tener más de 100 caracteres.");
                if (!Validaciones.EsEmailValido(cliente.Email))
                    throw new ArgumentException("El e-mail no es válido.");
            }

            cliente.Direccion = Validaciones.SacarEspacios(cliente.Direccion);
            if (cliente.Direccion != null && cliente.Direccion.Length > 150)
                throw new ArgumentException("La dirección no puede tener más de 150 caracteres.");
        }
    }
}
