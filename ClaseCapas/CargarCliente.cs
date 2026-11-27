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
    /// Alta de un cliente, o modificación si se abre con un cliente (desde Ver clientes).
    /// Desde Vender o Ver clientes ("Nuevo") se abre como ventana de diálogo.
    /// </summary>
    public partial class CargarCliente : Form
    {
        BLL.BLLCliente bll = new BLL.BLLCliente();

        // null = alta.
        BE.Cliente editando = null;

        public CargarCliente()
        {
            InitializeComponent();
        }

        public CargarCliente(BE.Cliente cliente)
        {
            InitializeComponent();
            editando = cliente;
        }

        private void CargarCliente_Load(object sender, EventArgs e)
        {
            if (editando == null)
                return;
            label1.Text = "MODIFICAR CLIENTE";
            btnRegistrar.Text = "Guardar";
            txtNombre.Text = editando.Nombre;
            txtApellido.Text = editando.Apellido;
            txtTelefono.Text = editando.Telefono;
            txtEmail.Text = editando.Email;
            txtDireccion.Text = editando.Direccion;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Cliente cliente = new BE.Cliente();
                if (editando != null)
                    cliente.IdCliente = editando.IdCliente;
                cliente.Nombre = txtNombre.Text;
                cliente.Apellido = txtApellido.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.Email = txtEmail.Text;
                cliente.Direccion = txtDireccion.Text;

                if (editando == null)
                {
                    bll.Alta(cliente);
                    Pantalla.Avisar("Cliente registrado.", "Clientes");
                }
                else
                {
                    bll.Modificar(cliente);
                    Pantalla.Avisar("Cliente modificado.", "Clientes");
                }

                if (this.Modal)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Clientes");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            txtNombre.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            Ventas ventas = new Ventas();
            ventas.Show();
            this.Close();
        }
    }
}
