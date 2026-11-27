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
    public partial class Ventas : Form
    {
        public Ventas()
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

        private void btnCargarCliente_Click(object sender, EventArgs e)
        {
            CargarCliente cliente = new CargarCliente();
            cliente.Show();
            this.Close();
        }

        private void btnVerClientes_Click(object sender, EventArgs e)
        {
            VerClientes clientes = new VerClientes();
            clientes.Show();
            this.Close();
        }

        private void btnAgenda_Click(object sender, EventArgs e)
        {
            Agenda agenda = new Agenda(false);
            agenda.Show();
            this.Close();
        }

        private void btnConsultarStock_Click(object sender, EventArgs e)
        {
            ConsultarStock stock = new ConsultarStock();
            stock.Show();
            this.Close();
        }

        private void btnRegistroVentas_Click(object sender, EventArgs e)
        {
            RegistrosVender registros = new RegistrosVender(false);
            registros.Show();
            this.Close();
        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            Vender vender = new Vender();
            vender.Show();
            this.Close();
        }
    }
}
