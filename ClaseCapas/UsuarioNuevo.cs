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
    /// Alta de un usuario, o modificación si se abre con un usuario (desde Ver usuarios).
    /// Al modificar, si la contraseña queda vacía se conserva la anterior.
    /// Con "soloContrasena" se usa para cambiar la contraseña (Olvidé mi contraseña).
    /// </summary>
    public partial class UsuarioNuevo : Form
    {
        BLL.BLLUsuario bll = new BLL.BLLUsuario();

        // null = alta.
        BE.Usuario editando = null;
        bool soloContrasena = false;

        public UsuarioNuevo()
        {
            InitializeComponent();
        }

        public UsuarioNuevo(BE.Usuario usuario, bool soloContrasena)
        {
            InitializeComponent();
            editando = usuario;
            this.soloContrasena = soloContrasena;
        }

        private void UsuarioNuevo_Load(object sender, EventArgs e)
        {
            try
            {
                cmbRol.DisplayMember = "Descripcion";
                cmbRol.ValueMember = "IdRol";
                cmbRol.DataSource = bll.ListarRoles();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Usuarios");
            }

            LimpiarCampos();
            if (editando == null)
                return;

            label1.Text = "MODIFICAR USUARIO";
            btnRegistrar.Text = "Guardar";
            txtNombre.Text = editando.Nombre;
            txtApellido.Text = editando.Apellido;
            txtDni.Text = editando.Dni;
            if (editando.FechaNacimiento != null)
            {
                dtpNacimiento.Value = editando.FechaNacimiento.Value;
                dtpNacimiento.Checked = true;
            }
            cmbRol.SelectedValue = editando.IdRol;
            txtTelefono.Text = editando.Telefono;
            txtEmail.Text = editando.Email;
            txtUsuario.Text = editando.NombreUsuario;

            if (soloContrasena)
            {
                // Solo se cambia la contraseña: el resto queda bloqueado.
                label1.Text = "NUEVA CONTRASEÑA";
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox || control is ComboBox || control is DateTimePicker)
                        control.Enabled = false;
                }
                txtContrasena.Enabled = true;
                txtConfirmar.Enabled = true;
                txtContrasena.Focus();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // La confirmación no se guarda: solo se controla que coincida.
                if (txtContrasena.Text != txtConfirmar.Text)
                    throw new ArgumentException("La contraseña y su confirmación no coinciden.");
                if (soloContrasena && txtContrasena.Text == "")
                    throw new ArgumentException("Ingrese la nueva contraseña.");

                BE.Usuario usuario = new BE.Usuario();
                if (editando != null)
                    usuario.IdUsuario = editando.IdUsuario;
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Dni = txtDni.Text;
                if (dtpNacimiento.Checked)
                    usuario.FechaNacimiento = dtpNacimiento.Value.Date;
                if (cmbRol.SelectedValue != null)
                    usuario.IdRol = (int)cmbRol.SelectedValue;
                usuario.Telefono = txtTelefono.Text;
                usuario.Email = txtEmail.Text;
                usuario.NombreUsuario = txtUsuario.Text;
                usuario.Contrasena = txtContrasena.Text;

                if (editando == null)
                {
                    bll.Alta(usuario);
                    Pantalla.Avisar("Usuario registrado.", "Usuarios");
                    LimpiarCampos();
                }
                else
                {
                    bll.Modificar(usuario);
                    if (soloContrasena)
                        Pantalla.Avisar("Contraseña cambiada.", "Usuarios");
                    else
                        Pantalla.Avisar("Usuario modificado.", "Usuarios");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Usuarios");
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            dtpNacimiento.Value = DateTime.Today;
            dtpNacimiento.Checked = false;
            cmbRol.SelectedIndex = -1;
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtUsuario.Text = "";
            txtContrasena.Text = "";
            txtConfirmar.Text = "";
            txtNombre.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (this.Modal)
            {
                this.Close();
                return;
            }
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }
    }
}
