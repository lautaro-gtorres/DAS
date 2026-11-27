using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    /// <summary>
    /// Muestra el comprobante de una venta. Se puede imprimir
    /// (con "Microsoft Print to PDF" queda como PDF) y guardar en XML.
    /// </summary>
    public partial class VerComprobante : Form
    {
        BLL.BLLComprobante bll = new BLL.BLLComprobante();
        int idVenta;
        BE.Comprobante comprobante;
        List<string> lineas;

        public VerComprobante(int idVenta)
        {
            InitializeComponent();
            this.idVenta = idVenta;
        }

        private void VerComprobante_Load(object sender, EventArgs e)
        {
            try
            {
                comprobante = bll.Generar(idVenta);
                lineas = bll.ArmarLineas(comprobante);
                txtComprobante.Lines = lineas.ToArray();
                this.Text = "Comprobante " + comprobante.NroComprobante;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                btnImprimir.Enabled = false;
                btnGuardarXML.Enabled = false;
            }
        }

        // ---- Imprimir ----

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument documento = new PrintDocument();
            documento.DocumentName = "Comprobante " + comprobante.NroComprobante;
            documento.PrintPage += new PrintPageEventHandler(ImprimirPagina);

            PrintPreviewDialog vistaPrevia = new PrintPreviewDialog();
            vistaPrevia.Document = documento;
            vistaPrevia.Width = 800;
            vistaPrevia.Height = 600;
            vistaPrevia.ShowDialog();
        }

        // Dibuja las líneas del comprobante en la hoja (entra en una sola página).
        private void ImprimirPagina(object sender, PrintPageEventArgs e)
        {
            Font letra = new Font("Courier New", 10);
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float alto = letra.GetHeight(e.Graphics);
            foreach (string linea in lineas)
            {
                e.Graphics.DrawString(linea, letra, Brushes.Black, x, y);
                y = y + alto;
            }
            letra.Dispose();
            e.HasMorePages = false;
        }

        // ---- XML ----

        private void btnGuardarXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialogo = new SaveFileDialog();
            dialogo.Filter = "Archivo XML (*.xml)|*.xml";
            dialogo.FileName = "Comprobante " + comprobante.NroComprobante + ".xml";
            if (dialogo.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                bll.ExportarXML(idVenta, dialogo.FileName);
                MessageBox.Show("Comprobante guardado en:\n" + dialogo.FileName, "Comprobante",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MostrarError(Exception ex)
        {
            if (ex is ArgumentException)
                MessageBox.Show(ex.Message, "Comprobante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Ocurrió un error al acceder a la base de datos.\n\n" + ex.Message, "Comprobante",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
