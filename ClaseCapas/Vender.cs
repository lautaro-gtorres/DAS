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

        // Venta que se está corrigiendo desde Registros (null = venta nueva).
        BE.Venta editando = null;

        // Para volver al menú desde el que se abrió (Gallinas o Ventas).
        bool desdeGallinas = false;

        public Vender()
        {
            InitializeComponent();
        }

        public Vender(bool desdeGallinas)
        {
            InitializeComponent();
            this.desdeGallinas = desdeGallinas;
        }

        // Corrección de una venta: solo se pueden cambiar el cliente y el precio unitario.
        public Vender(BE.Venta venta)
        {
            InitializeComponent();
            editando = venta;
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
                if (editando != null)
                    MostrarVenta();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Vender");
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

        // Elige en el combo el cliente con ese Id (o "Sin cliente").
        private void ElegirCliente(int? idCliente)
        {
            cmbCliente.SelectedIndex = 0;
            if (idCliente == null)
                return;
            for (int i = 1; i < cmbCliente.Items.Count; i++)
            {
                if (((BE.Cliente)cmbCliente.Items[i]).IdCliente == idCliente.Value)
                    cmbCliente.SelectedIndex = i;
            }
        }

        // Modo corrección: se muestra la venta y solo quedan habilitados cliente y precio.
        private void MostrarVenta()
        {
            label1.Text = "CORREGIR VENTA";
            btnVender.Text = "Guardar";
            cmbProducto.SelectedValue = editando.IdTipoProducto;
            if (editando.IdTipoGallina != null)
            {
                cmbTipoGallina.SelectedValue = editando.IdTipoGallina.Value;
                cmbRaza.SelectedValue = editando.IdRaza.Value;
                if (editando.Sexo == BE.Gallina.Macho)
                    cmbSexo.SelectedIndex = 1;
                else
                    cmbSexo.SelectedIndex = 0;
            }
            txtCantidad.Text = editando.Cantidad.ToString();
            txtPrecio.Text = editando.PrecioUnitario.ToString("0.##");
            ElegirCliente(editando.IdCliente);

            cmbProducto.Enabled = false;
            cmbTipoGallina.Enabled = false;
            cmbRaza.Enabled = false;
            cmbSexo.Enabled = false;
            txtCantidad.Enabled = false;
        }

        // Nuevo cliente: se carga y queda elegido en el combo (es el de Id más alto).
        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            CargarCliente nuevo = new CargarCliente();
            if (nuevo.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                CargarClientes();
                int idNuevo = 0;
                for (int i = 1; i < cmbCliente.Items.Count; i++)
                {
                    BE.Cliente cliente = (BE.Cliente)cmbCliente.Items[i];
                    if (cliente.IdCliente > idNuevo)
                        idNuevo = cliente.IdCliente;
                }
                ElegirCliente(idNuevo);
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Vender");
            }
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
                if (editando != null)
                {
                    // Corrección: se conserva el número y la fecha de la venta.
                    venta.IdVenta = editando.IdVenta;
                    venta.Fecha = editando.Fecha;
                    bll.Modificar(venta);
                    Pantalla.Avisar("Venta corregida.", "Vender");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }

                bll.Alta(venta);
                LimpiarCampos();

                // Venta registrada: se muestra su comprobante (Alta dejó el número en venta.IdVenta).
                VerComprobante comprobante = new VerComprobante(venta.IdVenta);
                comprobante.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Vender");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (editando != null)
            {
                this.Close();
                return;
            }
            LimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            if (desdeGallinas)
            {
                Gallinas gallinas = new Gallinas();
                gallinas.Show();
                this.Close();
                return;
            }
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
    }
}
