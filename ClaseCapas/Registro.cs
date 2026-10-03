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
    public partial class Registro : Form
    {
        BLL.Usuario usuarioBLL = new BLL.Usuario();

        // El login lo usa para completar el campo usuario después de registrarse.
        public string UsuarioCreado { get; private set; }

        public Registro()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Usuario usuario = new BE.Usuario();
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.Dni = txtDni.Text.Trim();
                usuario.NombreUsuario = txtUsuario.Text.Trim();
                usuario.Password = txtPassword.Text;

                usuarioBLL.RegistrarCliente(usuario, txtConfirmar.Text);

                MessageBox.Show("Cuenta creada. Ya podés iniciar sesión.", "Registro",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                UsuarioCreado = usuario.NombreUsuario;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
