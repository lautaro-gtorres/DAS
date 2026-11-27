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
    /// Compra de alimento (suministro) a un proveedor. Los kilos se suman al stock.
    /// Se abre desde Proveedores (Realizar pedido) o desde Consultar stock (Reponer).
    /// </summary>
    public partial class Comprar : Form
    {
        BLL.BLLCompra bll = new BLL.BLLCompra();

        // Alimento que viene elegido desde Consultar stock (0 = ninguno).
        int idAlimentoInicial = 0;

        public Comprar()
        {
            InitializeComponent();
        }

        public Comprar(int idAlimento)
        {
            InitializeComponent();
            idAlimentoInicial = idAlimento;
        }

        private void Comprar_Load(object sender, EventArgs e)
        {
            CargarCombos();
            LimpiarCampos();
            if (idAlimentoInicial > 0)
                cmbSuministro.SelectedValue = idAlimentoInicial;
        }

        private void CargarCombos()
        {
            try
            {
                cmbSuministro.DisplayMember = "NombreAlimento";
                cmbSuministro.ValueMember = "IdAlimento";
                cmbSuministro.DataSource = new BLL.BLLAlimento().Listar();

                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "IdProveedor";
                cmbProveedor.DataSource = new BLL.BLLProveedor().ListarActivos();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Comprar");
            }
        }

        // Al elegir el suministro se propone su proveedor habitual.
        private void cmbSuministro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSuministro.SelectedItem is BE.Alimento)
                cmbProveedor.SelectedValue = ((BE.Alimento)cmbSuministro.SelectedItem).IdProveedor;
        }

        private void CalcularTotal(object sender, EventArgs e)
        {
            decimal kilos;
            decimal precio;
            if (decimal.TryParse(txtKilogramos.Text.Trim(), out kilos) && decimal.TryParse(txtPrecio.Text.Trim(), out precio))
                txtTotal.Text = (kilos * precio).ToString("N2");
            else
                txtTotal.Text = "";
        }

        private void btnSolicitar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Compra compra = new BE.Compra();
                compra.Fecha = dtpFecha.Value.Date;
                if (cmbSuministro.SelectedValue != null)
                    compra.IdAlimento = (int)cmbSuministro.SelectedValue;
                if (cmbProveedor.SelectedValue != null)
                    compra.IdProveedor = (int)cmbProveedor.SelectedValue;

                decimal kilos;
                if (!decimal.TryParse(txtKilogramos.Text.Trim(), out kilos))
                    throw new ArgumentException("Los kilos tienen que ser un número.");
                compra.Kilogramos = kilos;

                decimal precio;
                if (!decimal.TryParse(txtPrecio.Text.Trim(), out precio))
                    throw new ArgumentException("El precio unitario tiene que ser un número.");
                compra.PrecioUnitario = precio;

                bll.Alta(compra);
                Pantalla.Avisar("Compra registrada: se sumaron " + kilos.ToString("N2") + " kg al stock.", "Comprar");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Comprar");
            }
        }

        private void btnNuevoSuministro_Click(object sender, EventArgs e)
        {
            CargarAlimento nuevo = new CargarAlimento();
            if (nuevo.ShowDialog(this) == DialogResult.OK)
                CargarCombos();
        }

        private void btnNuevoProveedor_Click(object sender, EventArgs e)
        {
            CargarProveedor nuevo = new CargarProveedor();
            if (nuevo.ShowDialog(this) == DialogResult.OK)
                CargarCombos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            cmbSuministro.SelectedIndex = -1;
            cmbProveedor.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
            txtKilogramos.Text = "";
            txtPrecio.Text = "";
            txtTotal.Text = "";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Desde Consultar stock (Reponer) se abre como diálogo: solo se cierra.
            if (this.Modal)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
            this.Close();
        }
    }
}
