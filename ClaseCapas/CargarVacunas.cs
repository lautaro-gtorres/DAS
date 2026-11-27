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

        // null = alta.
        BE.Vacunacion editando = null;

        public CargarVacunas()
        {
            InitializeComponent();
        }

        // Modificación de una vacunación (desde la Agenda).
        public CargarVacunas(BE.Vacunacion vacunacion)
        {
            InitializeComponent();
            editando = vacunacion;
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
                Pantalla.MostrarError(ex, "Vacunas");
            }

            if (editando != null)
            {
                label1.Text = "MODIFICAR VACUNACIÓN";
                btnRegistrar.Text = "Guardar";
                txtNombre.Text = editando.Nombre;
                txtCantidad.Text = editando.Cantidad.ToString();
                cmbCorral.SelectedValue = editando.IdCorral;
                dtpAplicacion.Value = editando.FechaAplicacion;
                if (editando.ProximaAplicacion != null)
                {
                    dtpProxima.Value = editando.ProximaAplicacion.Value;
                    dtpProxima.Checked = true;
                }
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Vacunacion vacunacion = LeerCampos();
                if (editando != null)
                {
                    vacunacion.IdVacunacion = editando.IdVacunacion;
                    bll.Modificar(vacunacion);
                    Pantalla.Avisar("Vacunación modificada.", "Vacunas");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                bll.Alta(vacunacion);
                Pantalla.Avisar("Vacunación registrada.", "Vacunas");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Vacunas");
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
    }
}
