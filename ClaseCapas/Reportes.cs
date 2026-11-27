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
    /// Reportes del último mes o del último año:
    ///  - Ventas totales (por producto).
    ///  - Compras de alimento (por alimento).
    ///  - Producción promedio de huevos (por corral y de toda la granja).
    /// Se recalcula al cambiar el período o el reporte.
    /// </summary>
    public partial class Reportes : Form
    {
        BLL.BLLReporte bll = new BLL.BLLReporte();

        const string ReporteVentas = "Ventas totales";
        const string ReporteCompras = "Compras de alimento";
        const string ReporteProduccion = "Producción promedio";

        public Reportes()
        {
            InitializeComponent();
        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvReporte);
            cmbReporte.Items.Add(ReporteVentas);
            cmbReporte.Items.Add(ReporteCompras);
            cmbReporte.Items.Add(ReporteProduccion);
            cmbReporte.SelectedIndex = 0;   // dispara Filtro_Cambio y genera el primero
        }

        // Cambió el período o el reporte elegido.
        private void Filtro_Cambio(object sender, EventArgs e)
        {
            // Al cambiar de radio button llegan dos eventos (el que se apaga y el que se prende): se usa solo el segundo.
            if (sender is RadioButton && !((RadioButton)sender).Checked)
                return;
            if (cmbReporte.SelectedItem == null)
                return;
            Generar();
        }

        private void Generar()
        {
            try
            {
                bool ultimoAnio = rbUltimoAnio.Checked;
                string reporte = cmbReporte.SelectedItem.ToString();
                string periodo = "del último mes";
                if (ultimoAnio)
                    periodo = "del último año";
                List<BE.LineaReporte> lineas;

                if (reporte == ReporteVentas)
                {
                    lineas = bll.Ventas(ultimoAnio);
                    Mostrar(lineas, "Producto", "Ventas", "Unidades", "Total $", "N0", "N2");
                    lblDescripcion.Text = "Ventas " + periodo;
                    lblResumen.Text = "Total vendido: $ " + Ultima(lineas).Valor.ToString("N2") + "  (" + Ultima(lineas).Registros + " ventas)";
                }
                else if (reporte == ReporteCompras)
                {
                    lineas = bll.Compras(ultimoAnio);
                    Mostrar(lineas, "Alimento", "Compras", "Kilos", "Total $", "N2", "N2");
                    lblDescripcion.Text = "Compras de alimento " + periodo;
                    lblResumen.Text = "Total comprado: $ " + Ultima(lineas).Valor.ToString("N2") + "  (" + Ultima(lineas).Cantidad.ToString("N2") + " kg)";
                }
                else
                {
                    lineas = bll.ProduccionPromedio(ultimoAnio);
                    Mostrar(lineas, "Corral", "Días con registro", "Huevos", "Promedio por día", "N0", "N1");
                    lblDescripcion.Text = "Producción de huevos " + periodo;
                    lblResumen.Text = "Promedio de la granja: " + Ultima(lineas).Valor.ToString("N1") + " huevos por día";
                }

                // Si en el período no hubo movimientos, el SP no devuelve filas.
                if (lineas.Count == 0)
                    lblResumen.Text = "No hay datos en el período.";
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Reportes");
            }
        }

        // Carga la grilla con los títulos y formatos de cada reporte. La fila TOTAL va en negrita.
        private void Mostrar(List<BE.LineaReporte> lineas, string concepto, string registros, string cantidad, string valor,
            string formatoCantidad, string formatoValor)
        {
            Pantalla.Llenar(dgvReporte, lineas);
            Pantalla.Titulo(dgvReporte, "Concepto", concepto);
            Pantalla.Titulo(dgvReporte, "Registros", registros);
            Pantalla.Titulo(dgvReporte, "Cantidad", cantidad);
            Pantalla.Titulo(dgvReporte, "Valor", valor);
            dgvReporte.Columns["Cantidad"].DefaultCellStyle.Format = formatoCantidad;
            dgvReporte.Columns["Valor"].DefaultCellStyle.Format = formatoValor;
            foreach (DataGridViewRow fila in dgvReporte.Rows)
            {
                if (((BE.LineaReporte)fila.DataBoundItem).Concepto == BLL.BLLReporte.Total)
                    fila.DefaultCellStyle.Font = new Font(dgvReporte.Font, FontStyle.Bold);
            }
        }

        // La última línea es la de TOTAL (si no hay filas, un total en cero).
        private BE.LineaReporte Ultima(List<BE.LineaReporte> lineas)
        {
            if (lineas.Count == 0)
                return new BE.LineaReporte(BLL.BLLReporte.Total);
            return lineas[lineas.Count - 1];
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }
    }
}
