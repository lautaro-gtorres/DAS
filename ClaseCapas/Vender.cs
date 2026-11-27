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
    /// Venta de huevos o gallinas.
    /// Si se venden gallinas, se elige tipo, raza y sexo, y esas gallinas se dan de baja.
    /// El cliente es opcional.
    /// </summary>
    public partial class Vender : Form
    {
        BLL.BLLVenta bll = new BLL.BLLVenta();

        // Primer ítem del combo de clientes: venta sin cliente.
        const string SinCliente = "(Sin cliente)";

        public Vender()
        {
            InitializeComponent();
        }

        private void Vender_Load(object sender, EventArgs e)
        {
            try
            {
                cmbProducto.DisplayMember = "Descripcion";
                cmbProducto.ValueMember = "IdTipoProducto";
                cmbProducto.DataSource = bll.ListarTiposProducto();

                BLL.BLLGallina bllGallina = new BLL.BLLGallina();
                cmbTipoGallina.DisplayMember = "Descripcion";
                cmbTipoGallina.ValueMember = "IdTipoGallina";
                cmbTipoGallina.DataSource = bllGallina.ListarTipos();

                BLL.BLLRaza bllRaza = new BLL.BLLRaza();
                cmbRaza.DisplayMember = "NombreRaza";
                cmbRaza.ValueMember = "IdRaza";
                cmbRaza.DataSource = bllRaza.Listar();

                cmbSexo.Items.Add("H - Hembra");
                cmbSexo.Items.Add("M - Macho");

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarClientes()
        {
            BLL.BLLCliente bllCliente = new BLL.BLLCliente();
            cmbCliente.Items.Clear();
            cmbCliente.Items.Add(SinCliente);
            foreach (BE.Cliente cliente in bllCliente.Listar())
                cmbCliente.Items.Add(cliente);
        }

        // ---- Selector de producto ----

        // Tipo, raza y sexo solo se usan cuando se venden gallinas.
        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool sonGallinas = EsVentaDeGallinas();
            cmbTipoGallina.Enabled = sonGallinas;
            cmbRaza.Enabled = sonGallinas;
            cmbSexo.Enabled = sonGallinas;
            if (!sonGallinas)
            {
                cmbTipoGallina.SelectedIndex = -1;
                cmbRaza.SelectedIndex = -1;
                cmbSexo.SelectedIndex = -1;
            }
        }

        private bool EsVentaDeGallinas()
        {
            if (cmbProducto.SelectedValue == null)
                return false;
            return (int)cmbProducto.SelectedValue == BLL.BLLVenta.ProductoGallina;
        }

        // ---- Total ----

        // Se recalcula cada vez que cambia la cantidad o el precio.
        private void CalcularTotal(object sender, EventArgs e)
        {
            int cantidad;
            decimal precio;
            if (int.TryParse(txtCantidad.Text.Trim(), out cantidad) && decimal.TryParse(txtPrecio.Text.Trim(), out precio))
                txtTotal.Text = (cantidad * precio).ToString("N2");
            else
                txtTotal.Text = "";
        }

        // ---- Botones ----

        private void btnVender_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Venta venta = LeerCampos();
                bll.Alta(venta);
                LimpiarCampos();

                // Venta registrada: se muestra su comprobante (Alta dejó el número en venta.IdVenta).
                VerComprobante comprobante = new VerComprobante(venta.IdVenta);
                comprobante.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Ventas ventas = new Ventas();
            ventas.Show();
            this.Close();
        }

        // ---- Campos ----

        private BE.Venta LeerCampos()
        {
            BE.Venta venta = new BE.Venta();
            venta.Fecha = DateTime.Today;
            if (cmbProducto.SelectedValue != null)
                venta.IdTipoProducto = (int)cmbProducto.SelectedValue;

            // El cliente es opcional.
            if (cmbCliente.SelectedItem is BE.Cliente)
                venta.IdCliente = ((BE.Cliente)cmbCliente.SelectedItem).IdCliente;

            if (EsVentaDeGallinas())
            {
                if (cmbTipoGallina.SelectedValue != null)
                    venta.IdTipoGallina = (int)cmbTipoGallina.SelectedValue;
                if (cmbRaza.SelectedValue != null)
                    venta.IdRaza = (int)cmbRaza.SelectedValue;
                if (cmbSexo.SelectedItem != null)
                    venta.Sexo = cmbSexo.SelectedItem.ToString()[0];
            }

            int cantidad;
            if (!int.TryParse(txtCantidad.Text.Trim(), out cantidad))
                throw new ArgumentException("La cantidad tiene que ser un número entero.");
            venta.Cantidad = cantidad;

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text.Trim(), out precio))
                throw new ArgumentException("El precio unitario tiene que ser un número.");
            venta.PrecioUnitario = precio;
            return venta;
        }

        private void LimpiarCampos()
        {
            cmbProducto.SelectedIndex = -1;
            cmbCliente.SelectedIndex = 0;
            cmbTipoGallina.SelectedIndex = -1;
            cmbRaza.SelectedIndex = -1;
            cmbSexo.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtTotal.Text = "";
            cmbProducto_SelectedIndexChanged(null, null);
        }

        // Los errores de validación (ArgumentException) se muestran como aviso;
        // cualquier otro, como error de conexión o de la base.
        private void MostrarError(Exception ex)
        {
            if (ex is ArgumentException)
                MessageBox.Show(ex.Message, "Vender", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Ocurrió un error al acceder a la base de datos.\n\n" + ex.Message, "Vender",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
