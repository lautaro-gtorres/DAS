using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Proveedor
    {
        private int nroProveedor;
        private string nombre;
        private string telefono;
        private string email;
        private string direccion;
        private bool activo;

        public int NroProveedor
        {
            get { return nroProveedor; }
            set { nroProveedor = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }
    }
}
