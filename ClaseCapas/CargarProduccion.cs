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
    /// Producción de huevos por corral y por día, en una sola pantalla:
    /// arriba se carga o edita y abajo está el listado. Al hacer clic en una fila se puede modificar o eliminar.
    /// </summary>
    public partial class CargarProduccion : Form
    {
        BLL.BLLProduccion bll = new BLL.BLLProduccion();

        // Id de la producción que se está editando; 0 = alta.
        int idEditando = 0;

        public CargarProduccion()
        {
            InitializeComponent();
        }

        private void CargarProduccion_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvProduccion);
            try
            {
                cmbCorral.DisplayMember = "NroCorral";
                cmbCorral.ValueMember = "IdCorral";
                cmbCorral.DataSource = new BLL.BLLCorral().Listar();
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Producción");
            }
        }

        private void CargarGrilla()
        {
            Pantalla.Llenar(dgvProduccion, bll.Listar());
            Pantalla.Titulo(dgvProduccion, "NroCorral", "Corral");
            Pantalla.Titulo(dgvProduccion, "CantidadHuevos", "Huevos");
            dgvProduccion.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            lblStock.Text = "Huevos disponibles para vender: " + new BLL.BLLVenta().StockHuevos();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Produccion produccion = new BE.Produccion();
                produccion.IdProduccion = idEditando;
                if (cmbCorral.SelectedValue != null)
                    produccion.IdCorral = (int)cmbCorral.SelectedValue;
                produccion.Fecha = dtpFecha.Value.Date;

                int huevos;
                if (!int.TryParse(txtHuevos.Text.Trim(), out huevos))
                    throw new ArgumentException("La cantidad de huevos tiene que ser un número entero.");
                produccion.CantidadHuevos = huevos;

                if (idEditando == 0)
                {
                    bll.Alta(produccion);
                    Pantalla.Avisar("Producción registrada.", "Producción");
                }
                else
                {
                    bll.Modificar(produccion);
                    Pantalla.Avisar("Producción modificada.", "Producción");
                }
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Producción");
            }
        }

        // Clic en una fila: pasa la producción a los campos para modificarla o eliminarla.
        private void dgvProduccion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            BE.Produccion produccion = (BE.Produccion)dgvProduccion.Rows[e.RowIndex].DataBoundItem;
            idEditando = produccion.IdProduccion;
            cmbCorral.SelectedValue = produccion.IdCorral;
            dtpFecha.Value = produccion.Fecha;
            txtHuevos.Text = produccion.CantidadHuevos.ToString();
            btnRegistrar.Text = "Guardar";
            btnEliminar.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idEditando == 0)
                return;
            if (!Pantalla.Confirmar("¿Eliminar la producción de " + cmbCorral.Text + " del " + dtpFecha.Value.ToString("dd/MM/yyyy") + "?", "Producción"))
                return;
            try
            {
                bll.Baja(idEditando);
                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Producción");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idEditando = 0;
            cmbCorral.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
            txtHuevos.Text = "";
            btnRegistrar.Text = "Registrar";
            btnEliminar.Enabled = false;
            dgvProduccion.ClearSelection();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }
    }
}
