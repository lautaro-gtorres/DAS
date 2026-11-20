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
    // Punto 3: Alta de viajes. Es el formulario principal.
    public partial class FrmViajes : Form
    {
        BLL.BLLViaje viajeBLL = new BLL.BLLViaje();
        BLL.BLLPasajero pasajeroBLL = new BLL.BLLPasajero();
        BLL.BLLVehiculo vehiculoBLL = new BLL.BLLVehiculo();

        public FrmViajes()
        {
            InitializeComponent();
        }

        void refresh()
        {
            cmbPasajero.DataSource = null;
            cmbPasajero.DataSource = pasajeroBLL.Listar();

            // Punto 2: vehículos desde la base, solo los que no están en un viaje
            cmbVehiculo.DataSource = null;
            cmbVehiculo.DataSource = vehiculoBLL.ListarDisponibles();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = viajeBLL.Listar();
        }

        private void FrmViajes_Load(object sender, EventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Si el pasajero no está registrado se carga en el ABM
        private void btnNuevoPasajero_Click(object sender, EventArgs e)
        {
            new FrmPasajeros().ShowDialog();
            refresh();
        }

        private void btnPasajeros_Click(object sender, EventArgs e)
        {
            new FrmPasajeros().ShowDialog();
            refresh();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            new FrmReportes().ShowDialog();
        }

        // Calcula el importe al escribir los km
        private void txtKm_TextoCambiado(object sender, EventArgs e)
        {
            decimal km;
            if (decimal.TryParse(txtKm.Texto, out km) && km > 0)
                txtImporte.Texto = viajeBLL.CalcularImporte(km).ToString("N2");
            else
                txtImporte.Texto = "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            bool destinoOk = txtDestino.Validar();
            bool kmOk = txtKm.Validar();
            if (!destinoOk || !kmOk) return;

            try
            {
                BE.Viaje viaje = new BE.Viaje();
                viaje.IdPasajero = cmbPasajero.SelectedItem == null ? 0 : ((BE.Pasajero)cmbPasajero.SelectedItem).IdPasajero;
                viaje.IdVehiculo = cmbVehiculo.SelectedItem == null ? 0 : ((BE.Vehiculo)cmbVehiculo.SelectedItem).IdVehiculo;
                viaje.Fecha = dtpFecha.Value;
                viaje.Destino = txtDestino.Texto;
                viaje.Kilometro = decimal.Parse(txtKm.Texto);

                int fa = viajeBLL.Alta(viaje);
                MessageBox.Show(fa != 0 ? "Viaje registrado" : "No se pudo registrar el viaje");
                txtDestino.Limpiar();
                txtKm.Limpiar();
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // El vehículo vuelve a estar disponible para otro viaje
        private void btnLiberar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un viaje.");
                return;
            }
            try
            {
                BE.Viaje viaje = (BE.Viaje)dataGridView1.SelectedRows[0].DataBoundItem;
                vehiculoBLL.Liberar(viaje.IdVehiculo);
                MessageBox.Show("Vehículo liberado");
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
