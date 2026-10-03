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
    public partial class Login : Form
    {
        BLL.Usuario usuarioBLL = new BLL.Usuario();

        public Login()
        {
            InitializeComponent();
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string password = txtPassword.Text;
            BE.Usuario usuario;

            // La consulta corre en segundo plano para que la ventana no quede "No responde"
            // mientras la base de Azure se despierta (puede tardar hasta un minuto).
            btnIngresar.Enabled = false;
            btnIngresar.Text = "Conectando...";
            this.UseWaitCursor = true;
            try
            {
                usuario = await Task.Run(() => usuarioBLL.Login(nombreUsuario, password));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            finally
            {
                btnIngresar.Enabled = true;
                btnIngresar.Text = "Ingresar";
                this.UseWaitCursor = false;
            }

            Form menu;
            if (usuario.Tipo == BLL.Usuario.TipoAdministrador)
                menu = new MenuAdministrador(usuario);
            else
                menu = new MenuCliente(usuario);

            // Se oculta el login mientras el menú está abierto;
            // "Cerrar sesión" cierra el menú y se vuelve acá.
            txtPassword.Clear();
            this.Hide();
            try
            {
                menu.ShowDialog();
            }
            finally
            {
                menu.Dispose();
                this.Show();
                txtPassword.Focus();
            }
        }

        private void lnkRegistro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Registro frm = new Registro())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    txtUsuario.Text = frm.UsuarioCreado;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
        }
    }
}
