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
    /// Registros con dos modos:
    ///  - Ventas (desde el menú Ventas): listado de ventas; se pueden corregir, anular y reimprimir el comprobante.
    ///  - Movimientos (desde Inicio, REGISTROS): ventas y compras juntas en una List&lt;BE.Movimiento&gt;.
    ///    Cada fila se arma con los métodos que redefine cada clase (TipoMovimiento, Detalle, Importe).
    /// </summary>
    public partial class RegistrosVender : Form
    {
        BLL.BLLVenta bllVenta = new BLL.BLLVenta();
        BLL.BLLMovimiento bllMovimiento = new BLL.BLLMovimiento();

        bool movimientos;

        public RegistrosVender(bool movimientos)
        {
            InitializeComponent();
            this.movimientos = movimientos;
        }

        private void RegistrosVender_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvRegistros);
            if (movimientos)
            {
                label1.Text = "REGISTROS";
                btnActualizar.Visible = false;
                btnEliminar.Text = "Anular";
            }
            else
            {
                label1.Text = "REGISTRO DE VENTAS";
            }
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                if (movimientos)
                    CargarMovimientos();
                else
                    CargarVentas();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Registros");
            }
        }

        private void CargarVentas()
        {
            List<BE.Venta> ventas = bllVenta.Listar();
            Pantalla.Llenar(dgvRegistros, ventas);
            Pantalla.Titulo(dgvRegistros, "TipoProducto", "Producto");
            Pantalla.Titulo(dgvRegistros, "TipoGallina", "Tipo");
            Pantalla.Titulo(dgvRegistros, "PrecioUnitario", "Precio unit.");
            dgvRegistros.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvRegistros.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            dgvRegistros.Columns["Total"].DefaultCellStyle.Format = "N2";

            decimal total = 0;
            foreach (BE.Venta venta in ventas)
                total = total + venta.Total;
            lblTotal.Text = ventas.Count + " ventas - Total vendido: $ " + total.ToString("N2");
        }

        // Lista polimórfica: ventas y compras juntas. Cada objeto responde según su clase.
        private void CargarMovimientos()
        {
            List<BE.Movimiento> lista = bllMovimiento.Listar();

            dgvRegistros.DataSource = null;
            dgvRegistros.Rows.Clear();
            dgvRegistros.Columns.Clear();
            dgvRegistros.Columns.Add("Fecha", "Fecha");
            dgvRegistros.Columns.Add("Tipo", "Tipo");
            dgvRegistros.Columns.Add("Detalle", "Detalle");
            dgvRegistros.Columns.Add("Importe", "Importe");
            dgvRegistros.Columns["Detalle"].FillWeight = 250;
            dgvRegistros.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (BE.Movimiento movimiento in lista)
            {
                int fila = dgvRegistros.Rows.Add(movimiento.Fecha.ToString("dd/MM/yyyy"), movimiento.TipoMovimiento(),
                    movimiento.Detalle(), movimiento.Importe().ToString("N2"));
                dgvRegistros.Rows[fila].Tag = movimiento;
                if (movimiento.Importe() < 0)
                    dgvRegistros.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
            }
            dgvRegistros.ClearSelection();
            lblTotal.Text = lista.Count + " movimientos - Balance (ventas - compras): $ " + bllMovimiento.Balance(lista).ToString("N2");
        }

        // El movimiento (Venta o Compra) de la fila elegida.
        private BE.Movimiento MovimientoElegido()
        {
            if (dgvRegistros.CurrentRow == null || dgvRegistros.SelectedRows.Count == 0)
                return null;
            if (movimientos)
                return (BE.Movimiento)dgvRegistros.CurrentRow.Tag;
            return (BE.Movimiento)dgvRegistros.CurrentRow.DataBoundItem;
        }

        private void btnComprobante_Click(object sender, EventArgs e)
        {
            BE.Movimiento movimiento = MovimientoElegido();
            if (!(movimiento is BE.Venta))
            {
                Pantalla.Avisar("Seleccione una venta de la lista.", "Registros");
                return;
            }
            VerComprobante comprobante = new VerComprobante(((BE.Venta)movimiento).IdVenta);
            comprobante.ShowDialog(this);
        }

        // Corrige cliente y precio de una venta.
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Movimiento movimiento = MovimientoElegido();
            if (!(movimiento is BE.Venta))
            {
                Pantalla.Avisar("Seleccione una venta de la lista.", "Registros");
                return;
            }
            Vender editar = new Vender((BE.Venta)movimiento);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        // Anula la venta (las gallinas vuelven) o la compra (se restan los kilos del stock).
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Movimiento movimiento = MovimientoElegido();
            if (movimiento == null)
            {
                Pantalla.Avisar("Seleccione un registro de la lista.", "Registros");
                return;
            }
            if (!Pantalla.Confirmar("¿Anular " + movimiento.TipoMovimiento().ToLower() + ": " + movimiento.Detalle() + "?", "Registros"))
                return;
            try
            {
                if (movimiento is BE.Venta)
                    bllVenta.Baja(((BE.Venta)movimiento).IdVenta);
                else
                    new BLL.BLLCompra().Baja(((BE.Compra)movimiento).IdCompra);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Registros");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (movimientos)
            {
                Inicio inicio = new Inicio();
                inicio.Show();
            }
            else
            {
                Ventas ventas = new Ventas();
                ventas.Show();
            }
            this.Close();
        }
    }
}
