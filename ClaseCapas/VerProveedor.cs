using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    /// <summary>
    /// Listado de proveedores (activos e inactivos). Actualizar abre el proveedor elegido para modificarlo.
    /// </summary>
    public partial class VerProveedor : Form
    {
        BLL.BLLProveedor bll = new BLL.BLLProveedor();

        public VerProveedor()
        {
            InitializeComponent();
        }

        private void VerProveedor_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvProveedores);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvProveedores, bll.Listar());
                Pantalla.Titulo(dgvProveedores, "Cuil", "CUIL");
                Pantalla.Titulo(dgvProveedores, "Direccion", "Dirección");
                Pantalla.Titulo(dgvProveedores, "Telefono", "Teléfono");
                Pantalla.Titulo(dgvProveedores, "Email", "E-mail");
                Pantalla.Titulo(dgvProveedores, "ServicioProducto", "Servicio/Producto");
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Proveedores");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Proveedor proveedor = (BE.Proveedor)Pantalla.Seleccionado(dgvProveedores);
            if (proveedor == null)
            {
                Pantalla.Avisar("Seleccione un proveedor de la lista.", "Proveedores");
                return;
            }
            CargarProveedor editar = new CargarProveedor(proveedor);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Proveedor proveedor = (BE.Proveedor)Pantalla.Seleccionado(dgvProveedores);
            if (proveedor == null)
            {
                Pantalla.Avisar("Seleccione un proveedor de la lista.", "Proveedores");
                return;
            }
            if (!Pantalla.Confirmar("¿Eliminar el proveedor " + proveedor.Nombre + "?", "Proveedores"))
                return;
            try
            {
                bll.Baja(proveedor.IdProveedor);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Proveedores");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
            this.Close();
        }
    }
}
