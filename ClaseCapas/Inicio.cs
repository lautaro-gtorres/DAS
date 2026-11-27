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

            // Los usuarios solo los maneja el administrador.
            bool esAdmin = BLL.Sesion.Instancia.EsAdministrador;
            btnUsuarioNuevo.Enabled = esAdmin;
            btnVerUsuarios.Enabled = esAdmin;
        }

        private void btnProduccion_Click(object sender, EventArgs e)
        {
            CargarProduccion produccion = new CargarProduccion();
            produccion.Show();
            this.Close();
        }

        // Registros: todos los movimientos (ventas y compras) en una sola lista.
        private void btnRegistros_Click(object sender, EventArgs e)
        {
            RegistrosVender registros = new RegistrosVender(true);
            registros.Show();
            this.Close();
        }

        private void btnAgenda_Click(object sender, EventArgs e)
        {
            Agenda agenda = new Agenda(true);
            agenda.Show();
            this.Close();
        }

        private void btnUsuarioNuevo_Click(object sender, EventArgs e)
        {
            UsuarioNuevo usuario = new UsuarioNuevo();
            usuario.Show();
            this.Close();
        }

        private void btnVerUsuarios_Click(object sender, EventArgs e)
        {
            VerUsuarios usuarios = new VerUsuarios();
            usuarios.Show();
            this.Close();
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
