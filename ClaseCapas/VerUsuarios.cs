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
    /// Listado de usuarios activos (solo administradores).
    /// Actualizar modifica el usuario elegido; "Olvidé mi contraseña" le pone una contraseña nueva;
    /// Eliminar lo da de baja (baja lógica).
    /// </summary>
    public partial class VerUsuarios : Form
    {
        BLL.BLLUsuario bll = new BLL.BLLUsuario();

        public VerUsuarios()
        {
            InitializeComponent();
        }

        private void VerUsuarios_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvUsuarios);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvUsuarios, bll.Listar());
                Pantalla.Ocultar(dgvUsuarios, "Contrasena");
                Pantalla.Ocultar(dgvUsuarios, "Activo");
                Pantalla.Titulo(dgvUsuarios, "NombreUsuario", "Usuario");
                Pantalla.Titulo(dgvUsuarios, "Dni", "DNI");
                Pantalla.Titulo(dgvUsuarios, "FechaNacimiento", "Nacimiento");
                Pantalla.Titulo(dgvUsuarios, "Telefono", "Teléfono");
                Pantalla.Titulo(dgvUsuarios, "Email", "E-mail");
                dgvUsuarios.Columns["FechaNacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Usuarios");
            }
        }

        private BE.Usuario UsuarioElegido()
        {
            BE.Usuario usuario = (BE.Usuario)Pantalla.Seleccionado(dgvUsuarios);
            if (usuario == null)
                Pantalla.Avisar("Seleccione un usuario de la lista.", "Usuarios");
            return usuario;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Usuario usuario = UsuarioElegido();
            if (usuario == null)
                return;
            UsuarioNuevo editar = new UsuarioNuevo(usuario, false);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnOlvideContrasena_Click(object sender, EventArgs e)
        {
            BE.Usuario usuario = UsuarioElegido();
            if (usuario == null)
                return;
            UsuarioNuevo cambiar = new UsuarioNuevo(usuario, true);
            cambiar.ShowDialog(this);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Usuario usuario = UsuarioElegido();
            if (usuario == null)
                return;
            if (!Pantalla.Confirmar("¿Dar de baja al usuario " + usuario.NombreUsuario + "?", "Usuarios"))
                return;
            try
            {
                bll.Baja(usuario.IdUsuario);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Usuarios");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }
    }
}
