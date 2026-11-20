using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    // Puntos 4 a 8
    public partial class FrmReportes : Form
    {
        BLL.BLLViaje viajeBLL = new BLL.BLLViaje();
        BLL.BLLVehiculo vehiculoBLL = new BLL.BLLVehiculo();

        public FrmReportes()
        {
            InitializeComponent();
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            try
            {
                lblCantidad.Text = "Cantidad de viajes: " + viajeBLL.Cantidad();     // punto 4
                lblTotal.Text = "Total de los viajes: $" + viajeBLL.Total().ToString("N2"); // punto 5
                cmbVehiculo.DataSource = vehiculoBLL.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Punto 6: se guarda el XML y se vuelve a leer para mostrarlo
        private void btnXml25000_Click(object sender, EventArgs e)
        {
            try
            {
                string ruta = Path.Combine(Application.StartupPath, "ViajesMayores25000.xml");
                viajeBLL.GuardarXmlMayores25000(ruta);
                dataGridView1.DataSource = viajeBLL.LeerXml(ruta);
                lblGrilla.Text = "Leído de " + ruta;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Punto 7
        private void btnXml20_Click(object sender, EventArgs e)
        {
            try
            {
                string ruta = Path.Combine(Application.StartupPath, "ViajesHasta20Km.xml");
                viajeBLL.GuardarXmlHasta20Km(ruta);
                dataGridView1.DataSource = viajeBLL.LeerXml(ruta);
                lblGrilla.Text = "Leído de " + ruta;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Punto 8
        private void btnPorVehiculo_Click(object sender, EventArgs e)
        {
            if (cmbVehiculo.SelectedItem == null) return;
            try
            {
                BE.Vehiculo v = (BE.Vehiculo)cmbVehiculo.SelectedItem;
                dataGridView1.DataSource = viajeBLL.ListarPorVehiculo(v.IdVehiculo);
                lblGrilla.Text = "Viajes del vehículo " + v;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
