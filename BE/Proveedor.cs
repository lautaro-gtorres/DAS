using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Tabla Proveedor.
    /// </summary>
    public class Proveedor
    {
        private int idProveedor;
        public int IdProveedor
        {
            get { return idProveedor; }
            set { idProveedor = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // N° de CUIL, con o sin guiones (ej: 20-12345678-9).
        private string cuil;
        public string Cuil
        {
            get { return cuil; }
            set { cuil = value; }
        }

        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        private string localidad;
        public string Localidad
        {
            get { return localidad; }
            set { localidad = value; }
        }

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // Qué servicio o producto le compramos.
        private string servicioProducto;
        public string ServicioProducto
        {
            get { return servicioProducto; }
            set { servicioProducto = value; }
        }

        // ESTADO del formulario: true = ACTIVO, false = INACTIVO.
        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        // Lo que se muestra cuando el objeto está en un ComboBox o ListBox.
        public override string ToString()
        {
            return Nombre;
        }
    }
}
