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
    public partial class Usuarios : Form
    {
        BLL.Usuario usuarioBLL = new BLL.Usuario();
        BE.Usuario usuarioLogueado;

        public Usuarios(BE.Usuario administrador)
        {
            InitializeComponent();
            usuarioLogueado = administrador;
        }

        void refresh()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = usuarioBLL.Listar();
            FormatearGrilla();
            dataGridView1.ClearSelection();
        }

        void FormatearGrilla()
        {
            if (dataGridView1.Columns.Count == 0)
                return;
            dataGridView1.Columns["Password"].Visible = false;
            dataGridView1.Columns["IdUsuario"].HeaderText = "ID";
            dataGridView1.Columns["Dni"].HeaderText = "DNI";
            dataGridView1.Columns["NombreUsuario"].HeaderText = "Usuario";
            dataGridView1.Columns["Estado"].HeaderText = "Activo";
            dataGridView1.Columns["Saldo"].DefaultCellStyle.Format = "C2";
            dataGridView1.Columns["Saldo"].DefaultCellStyle.FormatProvider = new CultureInfo("es-AR");
        }

        // Deja el formulario listo para un alta nueva.
        // El saldo solo se puede cargar como saldo inicial al crear el usuario;
        // después cambia únicamente por transferencias (punto 5 del enunciado).
        void Limpiar()
        {
            txtIdUsuario.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtUsuario.Text = "";
            txtPassword.Text = "";
            cmbTipo.SelectedItem = BLL.Usuario.TipoCliente;
            chkActivo.Checked = true;
            txtSaldo.Text = "0";
            txtSaldo.ReadOnly = false;
            label8.Text = "Saldo inicial";
            dataGridView1.ClearSelection();
        }

        BE.Usuario LeerFormulario()
        {
            BE.Usuario usuario = new BE.Usuario();
            usuario.IdUsuario = string.IsNullOrEmpty(txtIdUsuario.Text) ? 0 : int.Parse(txtIdUsuario.Text);
            usuario.Nombre = txtNombre.Text.Trim();
            usuario.Apellido = txtApellido.Text.Trim();
            usuario.Dni = txtDni.Text.Trim();
            usuario.NombreUsuario = txtUsuario.Text.Trim();
            usuario.Password = txtPassword.Text;
            usuario.Tipo = cmbTipo.SelectedItem == null ? "" : cmbTipo.SelectedItem.ToString();
            usuario.Estado = chkActivo.Checked;
            return usuario;
        }

        bool EsUsuarioLogueado()
        {
            return txtIdUsuario.Text == usuarioLogueado.IdUsuario.ToString();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            try
            {
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdUsuario.Text))
            {
                MessageBox.Show("Hay un usuario seleccionado. Tocá Limpiar para cargar uno nuevo.", "Usuarios");
                return;
            }
            try
            {
                int fa = 0;
                BE.Usuario usuario = LeerFormulario();
                double saldo;
                if (!double.TryParse(txtSaldo.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out saldo))
                {
                    MessageBox.Show("El saldo inicial no es un número válido.", "Usuarios");
                    return;
                }
                usuario.Saldo = saldo;
                fa = usuarioBLL.Alta(usuario);
                if (fa != 0)
                {
                    MessageBox.Show("Usuario agregado correctamente");
                }
                else
                {
                    MessageBox.Show("Error al agregar usuario");
                }
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdUsuario.Text))
            {
                MessageBox.Show("Seleccione un usuario de la lista.");
                return;
            }
            try
            {
                int fa = 0;
                BE.Usuario usuario = LeerFormulario();
                if (EsUsuarioLogueado() && (!usuario.Estado || usuario.Tipo != BLL.Usuario.TipoAdministrador))
                {
                    MessageBox.Show("No podés desactivarte ni quitarte el rol de administrador.", "Usuarios");
                    return;
                }
                // Contraseña vacía = se mantiene la actual
                fa = usuarioBLL.Modificar(usuario);
                if (fa != 0)
                {
                    MessageBox.Show("Usuario modificado correctamente");
                }
                else
                {
                    MessageBox.Show("Error al modificar usuario");
                }
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdUsuario.Text))
            {
                MessageBox.Show("Seleccione un usuario de la lista.");
                return;
            }
            if (EsUsuarioLogueado())
            {
                MessageBox.Show("No podés darte de baja a vos mismo.", "Usuarios");
                return;
            }
            DialogResult respuesta = MessageBox.Show("¿Dar de baja al usuario " + txtUsuario.Text + "? No va a poder iniciar sesión ni operar.",
                "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
                return;
            try
            {
                int fa = 0;
                fa = usuarioBLL.Baja(int.Parse(txtIdUsuario.Text));
                if (fa != 0)
                {
                    MessageBox.Show("Usuario dado de baja correctamente");
                }
                else
                {
                    MessageBox.Show("Error al dar de baja el usuario");
                }
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.SelectedRows.Count == 0)
                return;

            BE.Usuario usuario = (BE.Usuario)dataGridView1.SelectedRows[0].DataBoundItem;
            txtIdUsuario.Text = usuario.IdUsuario.ToString();
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtDni.Text = usuario.Dni;
            txtUsuario.Text = usuario.NombreUsuario;
            txtPassword.Text = "";   // vacía: si no se escribe otra, se mantiene la actual
            cmbTipo.SelectedItem = usuario.Tipo;
            chkActivo.Checked = usuario.Estado;
            txtSaldo.Text = usuario.Saldo.ToString("N2");
            txtSaldo.ReadOnly = true;
            label8.Text = "Saldo";
        }
    }
}
