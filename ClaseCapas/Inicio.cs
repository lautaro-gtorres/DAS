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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
            // Muestra el usuario logueado debajo del "Hola!".
            if (BLL.Sesion.Instancia.HaySesion)
                label3.Text = BLL.Sesion.Instancia.UsuarioActual.NombreUsuario;
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
            // Cierra la sesión y vuelve al login que quedó oculto (si no existe, crea uno).
            new BLL.BLLUsuario().Logout();
            Form1 login = null;
            foreach (Form abierto in Application.OpenForms)
            {
                if (abierto is Form1)
                    login = (Form1)abierto;
            }
            if (login == null)
                login = new Form1();
            login.Show();
            this.Close();
        }
    }
}
