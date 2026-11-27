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
    /// Listado de clientes activos. Nuevo y Actualizar abren CargarCliente;
    /// Eliminar da de baja al cliente (sus ventas se conservan).
    /// </summary>
    public partial class VerClientes : Form
    {
        BLL.BLLCliente bll = new BLL.BLLCliente();

        public VerClientes()
        {
            InitializeComponent();
        }

        private void VerClientes_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvClientes);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvClientes, bll.Listar());
                Pantalla.Ocultar(dgvClientes, "Activo");
                Pantalla.Titulo(dgvClientes, "Telefono", "Teléfono");
                Pantalla.Titulo(dgvClientes, "Email", "E-mail");
                Pantalla.Titulo(dgvClientes, "Direccion", "Dirección");
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Clientes");
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            CargarCliente nuevo = new CargarCliente();
            if (nuevo.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Cliente cliente = (BE.Cliente)Pantalla.Seleccionado(dgvClientes);
            if (cliente == null)
            {
                Pantalla.Avisar("Seleccione un cliente de la lista.", "Clientes");
                return;
            }
            CargarCliente editar = new CargarCliente(cliente);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Cliente cliente = (BE.Cliente)Pantalla.Seleccionado(dgvClientes);
            if (cliente == null)
            {
                Pantalla.Avisar("Seleccione un cliente de la lista.", "Clientes");
                return;
            }
            if (!Pantalla.Confirmar("¿Dar de baja al cliente " + cliente.NombreCompleto() + "?", "Clientes"))
                return;
            try
            {
                bll.Baja(cliente.IdCliente);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Clientes");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Ventas ventas = new Ventas();
            ventas.Show();
            this.Close();
        }
    }
}
