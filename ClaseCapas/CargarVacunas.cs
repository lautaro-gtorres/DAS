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
    /// Registro de una vacunación por corral.
    /// La próxima aplicación es opcional: se marca con la casilla del calendario.
    /// </summary>
    public partial class CargarVacunas : Form
    {
        BLL.BLLVacunacion bll = new BLL.BLLVacunacion();

        public CargarVacunas()
        {
            InitializeComponent();
        }

        private void CargarVacunas_Load(object sender, EventArgs e)
        {
            try
            {
                BLL.BLLCorral bllCorral = new BLL.BLLCorral();
                cmbCorral.DisplayMember = "NroCorral";
                cmbCorral.ValueMember = "IdCorral";
                cmbCorral.DataSource = bllCorral.Listar();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Vacunacion vacunacion = LeerCampos();
                bll.Alta(vacunacion);
                MessageBox.Show("Vacunación registrada.", "Vacunas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
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
            Gallinas gallinas = new Gallinas();
            gallinas.Show();
            this.Close();
        }

        private BE.Vacunacion LeerCampos()
        {
            BE.Vacunacion vacunacion = new BE.Vacunacion();
            vacunacion.Nombre = txtNombre.Text;
            if (cmbCorral.SelectedValue != null)
                vacunacion.IdCorral = (int)cmbCorral.SelectedValue;

            int cantidad;
            if (!int.TryParse(txtCantidad.Text.Trim(), out cantidad))
                throw new ArgumentException("La cantidad tiene que ser un número entero.");
            vacunacion.Cantidad = cantidad;

            vacunacion.FechaAplicacion = dtpAplicacion.Value.Date;
            if (dtpProxima.Checked)
                vacunacion.ProximaAplicacion = dtpProxima.Value.Date;
            return vacunacion;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCantidad.Text = "";
            cmbCorral.SelectedIndex = -1;
            dtpAplicacion.Value = DateTime.Today;
            dtpProxima.Value = DateTime.Today;
            dtpProxima.Checked = false;
            txtNombre.Focus();
        }

        // Los errores de validación (ArgumentException) se muestran como aviso;
        // cualquier otro, como error de conexión o de la base.
        private void MostrarError(Exception ex)
        {
            if (ex is ArgumentException)
                MessageBox.Show(ex.Message, "Vacunas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Ocurrió un error al acceder a la base de datos.\n\n" + ex.Message, "Vacunas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
