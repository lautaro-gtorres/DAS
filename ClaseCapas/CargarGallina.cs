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
    /// Alta de gallinas (de a una o "por mayor"), o modificación si se abre con una gallina (desde Ver gallinas).
    /// En la carga por mayor el código es el prefijo: GAL -> GAL-001, GAL-002, ...
    /// La fecha de nacimiento es opcional: se marca con la casilla del calendario.
    /// </summary>
    public partial class CargarGallina : Form
    {
        BLL.BLLGallina bll = new BLL.BLLGallina();

        // null = alta.
        BE.Gallina editando = null;
        bool porMayor = false;

        public CargarGallina()
        {
            InitializeComponent();
        }

        public CargarGallina(BE.Gallina gallina)
        {
            InitializeComponent();
            editando = gallina;
        }

        private void CargarGallina_Load(object sender, EventArgs e)
        {
            try
            {
                cmbTipo.DisplayMember = "Descripcion";
                cmbTipo.ValueMember = "IdTipoGallina";
                cmbTipo.DataSource = bll.ListarTipos();

                cmbRaza.DisplayMember = "NombreRaza";
                cmbRaza.ValueMember = "IdRaza";
                cmbRaza.DataSource = new BLL.BLLRaza().Listar();

                cmbCorral.DisplayMember = "NroCorral";
                cmbCorral.ValueMember = "IdCorral";
                cmbCorral.DataSource = new BLL.BLLCorral().Listar();

                cmbSexo.Items.Add("H - Hembra");
                cmbSexo.Items.Add("M - Macho");
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Gallinas");
            }

            LimpiarCampos();
            if (editando != null)
                MostrarGallina();
        }

        private void MostrarGallina()
        {
            label1.Text = "MODIFICAR GALLINA";
            btnRegistrar.Text = "Guardar";
            btnPorMayor.Visible = false;
            txtCodigo.Text = editando.CodGallina;
            cmbTipo.SelectedValue = editando.IdTipoGallina;
            cmbRaza.SelectedValue = editando.IdRaza;
            if (editando.Sexo == BE.Gallina.Macho)
                cmbSexo.SelectedIndex = 1;
            else
                cmbSexo.SelectedIndex = 0;
            cmbCorral.SelectedValue = editando.IdCorral;
            if (editando.FechaNacimiento != null)
            {
                dtpNacimiento.Value = editando.FechaNacimiento.Value;
                dtpNacimiento.Checked = true;
            }
            txtDetalle.Text = editando.Detalle;
        }

        // Pasa de carga individual a carga por mayor y al revés.
        private void btnPorMayor_Click(object sender, EventArgs e)
        {
            porMayor = !porMayor;
            lblCantidad.Visible = porMayor;
            nudCantidad.Visible = porMayor;
            if (porMayor)
            {
                btnPorMayor.Text = "Carga individual";
                label7.Text = "PREFIJO COD.:";
            }
            else
            {
                btnPorMayor.Text = "Carga por mayor";
                label7.Text = "COD. GALLINA:";
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Gallina gallina = LeerCampos();
                if (editando != null)
                {
                    bll.Modificar(gallina);
                    Pantalla.Avisar("Gallina modificada.", "Gallinas");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }

                if (porMayor)
                {
                    int cantidad = (int)nudCantidad.Value;
                    bll.AltaPorMayor(gallina, cantidad);
                    Pantalla.Avisar("Se registraron " + cantidad + " gallinas (" + gallina.CodGallina + "-001 a " +
                        gallina.CodGallina + "-" + cantidad.ToString("000") + ").", "Gallinas");
                }
                else
                {
                    bll.Alta(gallina);
                    Pantalla.Avisar("Gallina registrada.", "Gallinas");
                }
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Gallinas");
            }
        }

        private BE.Gallina LeerCampos()
        {
            BE.Gallina gallina = new BE.Gallina();
            if (editando != null)
                gallina.IdGallina = editando.IdGallina;
            gallina.CodGallina = txtCodigo.Text;
            if (cmbTipo.SelectedValue != null)
                gallina.IdTipoGallina = (int)cmbTipo.SelectedValue;
            if (cmbRaza.SelectedValue != null)
                gallina.IdRaza = (int)cmbRaza.SelectedValue;
            if (cmbSexo.SelectedItem != null)
                gallina.Sexo = cmbSexo.SelectedItem.ToString()[0];
            if (cmbCorral.SelectedValue != null)
                gallina.IdCorral = (int)cmbCorral.SelectedValue;
            if (dtpNacimiento.Checked)
                gallina.FechaNacimiento = dtpNacimiento.Value.Date;
            gallina.Detalle = txtDetalle.Text;
            return gallina;
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

        private void LimpiarCampos()
        {
            txtCodigo.Text = "";
            cmbTipo.SelectedIndex = -1;
            cmbRaza.SelectedIndex = -1;
            cmbSexo.SelectedIndex = -1;
            cmbCorral.SelectedIndex = -1;
            dtpNacimiento.Value = DateTime.Today;
            dtpNacimiento.Checked = false;
            txtDetalle.Text = "";
            txtCodigo.Focus();
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
