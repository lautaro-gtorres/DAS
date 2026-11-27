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
    /// Alta de un alimento (stock en kilogramos), o modificación si se abre con un alimento
    /// (desde Consultar stock). Solo se ofrecen proveedores activos.
    /// </summary>
    public partial class CargarAlimento : Form
    {
        BLL.BLLAlimento bll = new BLL.BLLAlimento();

        // null = alta.
        BE.Alimento editando = null;

        public CargarAlimento()
        {
            InitializeComponent();
        }

        public CargarAlimento(BE.Alimento alimento)
        {
            InitializeComponent();
            editando = alimento;
        }

        private void CargarAlimento_Load(object sender, EventArgs e)
        {
            try
            {
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "IdProveedor";
                cmbProveedor.DataSource = new BLL.BLLProveedor().ListarActivos();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Alimentos");
            }

            LimpiarCampos();
            if (editando != null)
            {
                label1.Text = "MODIFICAR ALIMENTO";
                btnRegistrar.Text = "Guardar";
                txtNombre.Text = editando.NombreAlimento;
                txtKilogramos.Text = editando.Kilogramos.ToString("0.##");
                cmbProveedor.SelectedValue = editando.IdProveedor;
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Alimento alimento = new BE.Alimento();
                if (editando != null)
                    alimento.IdAlimento = editando.IdAlimento;
                alimento.NombreAlimento = txtNombre.Text;
                if (cmbProveedor.SelectedValue != null)
                    alimento.IdProveedor = (int)cmbProveedor.SelectedValue;

                // Los kilos son opcionales al dar de alta (se suman después con las compras).
                string kilos = txtKilogramos.Text.Trim();
                if (kilos != "")
                {
                    decimal valor;
                    if (!decimal.TryParse(kilos, out valor))
                        throw new ArgumentException("Los kilogramos tienen que ser un número.");
                    alimento.Kilogramos = valor;
                }

                if (editando == null)
                {
                    bll.Alta(alimento);
                    Pantalla.Avisar("Alimento registrado.", "Alimentos");
                }
                else
                {
                    bll.Modificar(alimento);
                    Pantalla.Avisar("Alimento modificado.", "Alimentos");
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
                Pantalla.MostrarError(ex, "Alimentos");
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
            txtKilogramos.Text = "";
            cmbProveedor.SelectedIndex = -1;
            txtNombre.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            Gallinas gallinas = new Gallinas();
            gallinas.Show();
            this.Close();
        }
    }
}
