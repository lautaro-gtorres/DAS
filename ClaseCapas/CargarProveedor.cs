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
    /// Alta de un proveedor, o modificación si se abre con un proveedor (desde Ver proveedores).
    /// </summary>
    public partial class CargarProveedor : Form
    {
        BLL.BLLProveedor bll = new BLL.BLLProveedor();

        // null = alta de uno nuevo.
        BE.Proveedor editando = null;

        public CargarProveedor()
        {
            InitializeComponent();
        }

        public CargarProveedor(BE.Proveedor proveedor)
        {
            InitializeComponent();
            editando = proveedor;
        }

        private void CargarProveedor_Load(object sender, EventArgs e)
        {
            if (editando == null)
            {
                rbActivo.Checked = true;
                return;
            }

            label1.Text = "MODIFICAR PROVEEDOR";
            btnRegistrar.Text = "Guardar";
            txtNombre.Text = editando.Nombre;
            txtCuil.Text = editando.Cuil;
            txtDireccion.Text = editando.Direccion;
            txtLocalidad.Text = editando.Localidad;
            txtTelefono.Text = editando.Telefono;
            txtEmail.Text = editando.Email;
            txtServicio.Text = editando.ServicioProducto;
            rbActivo.Checked = editando.Activo;
            rbInactivo.Checked = !editando.Activo;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Proveedor proveedor = new BE.Proveedor();
                if (editando != null)
                    proveedor.IdProveedor = editando.IdProveedor;
                proveedor.Nombre = txtNombre.Text;
                proveedor.Cuil = txtCuil.Text;
                proveedor.Direccion = txtDireccion.Text;
                proveedor.Localidad = txtLocalidad.Text;
                proveedor.Telefono = txtTelefono.Text;
                proveedor.Email = txtEmail.Text;
                proveedor.ServicioProducto = txtServicio.Text;
                proveedor.Activo = rbActivo.Checked;

                if (editando == null)
                {
                    bll.Alta(proveedor);
                    Pantalla.Avisar("Proveedor registrado.", "Proveedores");
                }
                else
                {
                    bll.Modificar(proveedor);
                    Pantalla.Avisar("Proveedor modificado.", "Proveedores");
                }

                // Si se abrió como ventana de diálogo (desde otra pantalla), se cierra y avisa que hubo cambios.
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
                Pantalla.MostrarError(ex, "Proveedores");
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCuil.Text = "";
            txtDireccion.Text = "";
            txtLocalidad.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtServicio.Text = "";
            rbActivo.Checked = true;
            txtNombre.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
            this.Close();
        }
    }
}
