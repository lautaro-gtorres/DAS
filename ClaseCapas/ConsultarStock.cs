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
    /// Stock de alimentos (en kilogramos), más un resumen de huevos y gallinas disponibles.
    /// Reponer abre Comprar con el alimento elegido; Actualizar lo modifica; Eliminar lo borra.
    /// </summary>
    public partial class ConsultarStock : Form
    {
        BLL.BLLAlimento bll = new BLL.BLLAlimento();

        public ConsultarStock()
        {
            InitializeComponent();
        }

        private void ConsultarStock_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvStock);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvStock, bll.Listar());
                Pantalla.Titulo(dgvStock, "NombreAlimento", "Alimento");
                dgvStock.Columns["Kilogramos"].DefaultCellStyle.Format = "N2";

                int huevos = new BLL.BLLVenta().StockHuevos();
                int gallinas = new BLL.BLLGallina().Listar().Count;
                lblResumen.Text = "Huevos disponibles: " + huevos + "     Gallinas activas: " + gallinas;
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Stock");
            }
        }

        private BE.Alimento AlimentoElegido()
        {
            BE.Alimento alimento = (BE.Alimento)Pantalla.Seleccionado(dgvStock);
            if (alimento == null)
                Pantalla.Avisar("Seleccione un alimento de la lista.", "Stock");
            return alimento;
        }

        private void btnReponer_Click(object sender, EventArgs e)
        {
            BE.Alimento alimento = AlimentoElegido();
            if (alimento == null)
                return;
            Comprar comprar = new Comprar(alimento.IdAlimento);
            comprar.ShowDialog(this);
            CargarGrilla();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Alimento alimento = AlimentoElegido();
            if (alimento == null)
                return;
            CargarAlimento editar = new CargarAlimento(alimento);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Alimento alimento = AlimentoElegido();
            if (alimento == null)
                return;
            if (!Pantalla.Confirmar("¿Eliminar el alimento " + alimento.NombreAlimento + "?", "Stock"))
                return;
            try
            {
                bll.Baja(alimento.IdAlimento);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Stock");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Ventas ventas = new Ventas();
            ventas.Show();
            this.Close();
        }
    }
}
