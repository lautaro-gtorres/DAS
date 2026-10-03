using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    public partial class MenuCliente : Form
    {
        BE.Usuario usuario;
        BLL.Usuario usuarioBLL = new BLL.Usuario();

        public MenuCliente(BE.Usuario usuarioLogueado)
        {
            InitializeComponent();
            usuario = usuarioLogueado;
        }

        void MostrarDatos()
        {
            lblUsuarioLogueado.Text = usuario.Nombre + " " + usuario.Apellido + " (" + usuario.NombreUsuario + ")";
            lblSaldo.Text = usuario.Saldo.ToString("C2", new CultureInfo("es-AR"));
        }

        // Vuelve a leer el usuario de la base (por ejemplo, después de transferir).
        void ActualizarSaldo()
        {
            try
            {
                BE.Usuario actualizado = usuarioBLL.ObtenerPorId(usuario.IdUsuario);
                if (actualizado != null)
                    usuario = actualizado;
                MostrarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuCliente_Load(object sender, EventArgs e)
        {
            MostrarDatos();
        }

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            using (Transferencia frm = new Transferencia(usuario))
            {
                frm.ShowDialog(this);
            }
            ActualizarSaldo();
        }

        private void btnEstadoCuenta_Click(object sender, EventArgs e)
        {
            using (EstadoCuenta frm = new EstadoCuenta(usuario))
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
