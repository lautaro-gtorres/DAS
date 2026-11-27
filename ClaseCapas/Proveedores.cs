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
    public partial class Proveedores : Form
    {
        public Proveedores()
        {
            InitializeComponent();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }

        private void btnGallinas_Click(object sender, EventArgs e)
        {
            Gallinas gallinas = new Gallinas();
            gallinas.Show();
            this.Close();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.Show();
            this.Close();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            Ventas ventas = new Ventas();
            ventas.Show();
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }

        private void btnCargarProveedor_Click(object sender, EventArgs e)
        {
            CargarProveedor cargar = new CargarProveedor();
            cargar.Show();
            this.Close();
        }

        private void btnVerProveedores_Click(object sender, EventArgs e)
        {
            VerProveedor ver = new VerProveedor();
            ver.Show();
            this.Close();
        }

        // Realizar pedido = comprar alimento a un proveedor.
        private void btnRealizarPedido_Click(object sender, EventArgs e)
        {
            Comprar comprar = new Comprar();
            comprar.Show();
            this.Close();
        }

        // Consultar a proveedores = historial de compras.
        private void btnConsultarProveedores_Click(object sender, EventArgs e)
        {
            VerCompras compras = new VerCompras();
            compras.Show();
            this.Close();
        }
    }
}
