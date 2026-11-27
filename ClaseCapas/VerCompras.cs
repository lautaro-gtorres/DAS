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
    /// Historial de compras a proveedores ("Consultar a proveedores").
    /// Anular una compra resta los kilos del stock.
    /// </summary>
    public partial class VerCompras : Form
    {
        BLL.BLLCompra bll = new BLL.BLLCompra();

        public VerCompras()
        {
            InitializeComponent();
        }

        private void VerCompras_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvCompras);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                List<BE.Compra> compras;
                if (txtBuscar.Text.Trim() == "")
                {
                    compras = bll.Listar();
                }
                else
                {
                    BE.Compra filtro = new BE.Compra();
                    filtro.Proveedor = txtBuscar.Text.Trim();
                    compras = bll.Buscar(filtro);
                }
                Pantalla.Llenar(dgvCompras, compras);
                Pantalla.Titulo(dgvCompras, "PrecioUnitario", "Precio x kg");
                dgvCompras.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvCompras.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
                dgvCompras.Columns["Total"].DefaultCellStyle.Format = "N2";
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Compras");
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            BE.Compra compra = (BE.Compra)Pantalla.Seleccionado(dgvCompras);
            if (compra == null)
            {
                Pantalla.Avisar("Seleccione una compra de la lista.", "Compras");
                return;
            }
            if (!Pantalla.Confirmar("¿Anular la compra de " + compra.Kilogramos.ToString("N2") + " kg de " + compra.Alimento +
                "?\nSe van a restar del stock.", "Compras"))
                return;
            try
            {
                bll.Baja(compra.IdCompra);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Compras");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
            this.Close();
        }
    }
}
