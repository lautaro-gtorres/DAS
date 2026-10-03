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
    public partial class Transferencia : Form
    {
        BLL.Transferencia transferenciaBLL = new BLL.Transferencia();
        BLL.Usuario usuarioBLL = new BLL.Usuario();
        BE.Usuario usuario;
        CultureInfo cultura = new CultureInfo("es-AR");

        public Transferencia(BE.Usuario usuarioLogueado)
        {
            InitializeComponent();
            usuario = usuarioLogueado;
        }

        void MostrarSaldo()
        {
            BE.Usuario actualizado = usuarioBLL.ObtenerPorId(usuario.IdUsuario);
            if (actualizado != null)
                usuario = actualizado;
            lblSaldoDisponible.Text = usuario.Saldo.ToString("C2", cultura);
        }

        void Limpiar()
        {
            txtDestinatario.Text = "";
            nudImporte.Value = 0;
            txtConcepto.Text = "";
            txtDestinatario.Focus();
        }

        private void Transferencia_Load(object sender, EventArgs e)
        {
            try
            {
                MostrarSaldo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Transferencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            string destinatario = txtDestinatario.Text.Trim();
            double importe = (double)nudImporte.Value;

            try
            {
                // 1) Validaciones (destinatario existe, activo, distinto, importe > 0, saldo)
                BE.Usuario receptor = transferenciaBLL.Validar(usuario.IdUsuario, destinatario, importe);

                // 2) Confirmación
                DialogResult respuesta = MessageBox.Show(
                    "¿Confirmás la transferencia de " + importe.ToString("C2", cultura) + " a " +
                    receptor.Nombre + " " + receptor.Apellido + " (" + receptor.NombreUsuario + ")?",
                    "Confirmar transferencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta != DialogResult.Yes)
                    return;

                // 3) Transferencia en una única transacción (COMMIT / ROLLBACK en el DAL)
                transferenciaBLL.Realizar(usuario.IdUsuario, destinatario, importe, txtConcepto.Text);

                MessageBox.Show("Transferencia realizada correctamente.", "Transferencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                MostrarSaldo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "La transferencia no se realizó",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try
                {
                    MostrarSaldo();
                }
                catch (Exception)
                {
                    // Si tampoco se puede leer el saldo, ya se mostró el error principal.
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
