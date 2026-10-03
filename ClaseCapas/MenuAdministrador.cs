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
    public partial class MenuAdministrador : Form
    {
        BE.Usuario usuario;

        public MenuAdministrador(BE.Usuario usuarioLogueado)
        {
            InitializeComponent();
            usuario = usuarioLogueado;
        }

        private void MenuAdministrador_Load(object sender, EventArgs e)
        {
            lblUsuarioLogueado.Text = "Administrador: " + usuario.Nombre + " " + usuario.Apellido;
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            using (Usuarios frm = new Usuarios(usuario))
            {
                frm.ShowDialog(this);
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
