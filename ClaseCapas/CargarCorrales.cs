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
    /// ABM de corrales en una sola pantalla:
    /// arriba se carga o edita un corral y abajo está el listado.
    /// Al hacer clic en una fila, sus datos pasan a los campos para modificarlo o eliminarlo.
    /// </summary>
    public partial class CargarCorrales : Form
    {
        BLL.BLLCorral bll = new BLL.BLLCorral();

        // Id del corral que se está editando; 0 = alta de uno nuevo.
        private int idEditando = 0;

        public CargarCorrales()
        {
            InitializeComponent();
        }

        private void CargarCorrales_Load(object sender, EventArgs e)
        {
            try
            {
                cmbTipo.DisplayMember = "Descripcion";
                cmbTipo.ValueMember = "IdTipoCorral";
                cmbTipo.DataSource = bll.ListarTipos();
                cmbTipo.SelectedIndex = -1;
                cmbUbicacion.DisplayMember = "Nombre";
                cmbUbicacion.ValueMember = "IdUbicacion";
                cmbUbicacion.DataSource = bll.ListarUbicaciones();
                cmbUbicacion.SelectedIndex = -1;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Corrales");
            }
        }

        // ---- Botones ----

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Corral corral = LeerCampos();
                if (idEditando == 0)
                {
                    bll.Alta(corral);
                    MessageBox.Show("Corral registrado.", "Corrales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bll.Modificar(corral);
                    MessageBox.Show("Corral modificado.", "Corrales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Corrales");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idEditando == 0)
                return;

            DialogResult respuesta = MessageBox.Show("¿Eliminar el corral " + txtNroCorral.Text + "?", "Corrales",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                bll.Baja(idEditando);
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Corrales");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // Muestra la granja como árbol (Granja → Ubicación → Corral) con sus totales.
        private void btnEstructura_Click(object sender, EventArgs e)
        {
            EstructuraGranja estructura = new EstructuraGranja();
            estructura.ShowDialog(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Gallinas gallinas = new Gallinas();
            gallinas.Show();
            this.Close();
        }

        // ---- Grilla y búsqueda ----

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Corrales");
            }
        }

        // Clic en una fila: pasa el corral a los campos para modificarlo o eliminarlo.
        private void dgvCorrales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            BE.Corral corral = (BE.Corral)dgvCorrales.Rows[e.RowIndex].DataBoundItem;
            idEditando = corral.IdCorral;
            txtNroCorral.Text = corral.NroCorral;
            cmbTipo.SelectedValue = corral.IdTipoCorral;
            if (corral.Capacidad == null)
                txtCapacidad.Text = "";
            else
                txtCapacidad.Text = corral.Capacidad.Value.ToString();
            if (corral.IdUbicacion == null)
                cmbUbicacion.SelectedIndex = -1;
            else
                cmbUbicacion.SelectedValue = corral.IdUbicacion.Value;

            btnRegistrar.Text = "Guardar";
            btnEliminar.Enabled = true;
        }

        private void CargarGrilla()
        {
            BE.Corral filtro = new BE.Corral();
            filtro.NroCorral = txtBuscar.Text.Trim();
            List<BE.Corral> corrales;
            if (filtro.NroCorral == "")
                corrales = bll.Listar();
            else
                corrales = bll.Buscar(filtro);

            dgvCorrales.DataSource = null;
            dgvCorrales.DataSource = corrales;
            dgvCorrales.Columns["IdCorral"].Visible = false;
            dgvCorrales.Columns["IdTipoCorral"].Visible = false;
            dgvCorrales.Columns["IdUbicacion"].Visible = false;
            dgvCorrales.Columns["Ubicacion"].HeaderText = "Ubicación";
            dgvCorrales.Columns["NroCorral"].HeaderText = "Número";
            dgvCorrales.Columns["TipoCorral"].HeaderText = "Tipo";
            dgvCorrales.Columns["Capacidad"].HeaderText = "Capacidad máx.";
            dgvCorrales.ClearSelection();
        }

        // ---- Campos ----

        private BE.Corral LeerCampos()
        {
            BE.Corral corral = new BE.Corral();
            corral.IdCorral = idEditando;
            corral.NroCorral = txtNroCorral.Text;
            if (cmbTipo.SelectedValue != null)
                corral.IdTipoCorral = (int)cmbTipo.SelectedValue;
            // La ubicación es opcional.
            if (cmbUbicacion.SelectedValue != null)
                corral.IdUbicacion = (int)cmbUbicacion.SelectedValue;

            // La capacidad es opcional; si se carga tiene que ser un número entero.
            string capacidad = txtCapacidad.Text.Trim();
            if (capacidad != "")
            {
                int valor;
                if (!int.TryParse(capacidad, out valor))
                    throw new ArgumentException("La capacidad tiene que ser un número entero.");
                corral.Capacidad = valor;
            }
            return corral;
        }

        private void LimpiarCampos()
        {
            idEditando = 0;
            txtNroCorral.Text = "";
            txtCapacidad.Text = "";
            cmbTipo.SelectedIndex = -1;
            cmbUbicacion.SelectedIndex = -1;
            btnRegistrar.Text = "Registrar";
            btnEliminar.Enabled = false;
            dgvCorrales.ClearSelection();
            txtNroCorral.Focus();
        }
    }
}
