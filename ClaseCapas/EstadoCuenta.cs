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
    public partial class EstadoCuenta : Form
    {
        BLL.Movimiento movimientoBLL = new BLL.Movimiento();
        BLL.Usuario usuarioBLL = new BLL.Usuario();
        BE.Usuario usuario;
        CultureInfo cultura = new CultureInfo("es-AR");

        public EstadoCuenta(BE.Usuario usuarioLogueado)
        {
            InitializeComponent();
            usuario = usuarioLogueado;
        }

        void Cargar()
        {
            BE.Usuario actualizado = usuarioBLL.ObtenerPorId(usuario.IdUsuario);
            if (actualizado != null)
                usuario = actualizado;

            lblTitular.Text = "Titular: " + usuario.Nombre + " " + usuario.Apellido + " (" + usuario.NombreUsuario + ")";
            lblSaldo.Text = usuario.Saldo.ToString("C2", cultura);

            // Solo los movimientos del usuario logueado
            dgvMovimientos.DataSource = null;
            dgvMovimientos.DataSource = movimientoBLL.ListarPorUsuario(usuario.IdUsuario);
            FormatearGrilla();
            dgvMovimientos.ClearSelection();
        }

        void FormatearGrilla()
        {
            if (dgvMovimientos.Columns.Count == 0)
                return;
            dgvMovimientos.Columns["IdMovimiento"].Visible = false;
            dgvMovimientos.Columns["IdUsuario"].Visible = false;
            dgvMovimientos.Columns["IdUsuarioRelacionado"].Visible = false;
            dgvMovimientos.Columns["IdTransferencia"].Visible = false;

            dgvMovimientos.Columns["FechaHora"].HeaderText = "Fecha y hora";
            dgvMovimientos.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvMovimientos.Columns["Tipo"].HeaderText = "Tipo";
            dgvMovimientos.Columns["Importe"].HeaderText = "Importe";
            dgvMovimientos.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovimientos.Columns["UsuarioRelacionado"].HeaderText = "Usuario relacionado";
            dgvMovimientos.Columns["Concepto"].HeaderText = "Concepto";
        }

        private void EstadoCuenta_Load(object sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Estado de cuenta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Estado de cuenta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Egresos en rojo con signo "-", ingresos en verde con "+".
        private void dgvMovimientos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMovimientos.Columns[e.ColumnIndex].Name != "Importe" || e.Value == null)
                return;

            BE.Movimiento movimiento = dgvMovimientos.Rows[e.RowIndex].DataBoundItem as BE.Movimiento;
            if (movimiento == null)
                return;

            bool egreso = movimiento.Tipo == "Egreso";
            e.Value = (egreso ? "- " : "+ ") + movimiento.Importe.ToString("C2", cultura);
            e.CellStyle.ForeColor = egreso ? Color.FromArgb(185, 28, 28) : Color.FromArgb(21, 128, 61);
            e.FormattingApplied = true;
        }
    }
}
